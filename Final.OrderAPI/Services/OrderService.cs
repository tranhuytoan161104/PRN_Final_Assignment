using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Enums;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.OrderAPI.DTOs;

namespace Final.OrderAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        /// <summary>
        /// Phương thức tạo đơn hàng từ giỏ hàng của người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <param name="createOrderDto">Thông tin đơn hàng cần tạo.</param> 
        /// <returns>Trả về đơn hàng đã được tạo.</returns>
        /// <exception cref="InvalidOperationException">Nếu không có sản phẩm nào được chọn để thanh toán hoặc nếu sản phẩm không đủ hàng trong kho.</exception>
        public async Task<OrderDto> CreateOrderFromUserCartAsync(long userId, CreateOrderDto createOrderDto)
        {
            await using var transaction = await _orderRepository.BeginTransactionAsync();
            try
            {
                var cart = await _shoppingCartRepository.GetOrCreateCartByUserIdAsync(userId);
                var itemsToCheckout = cart.Items.Where(ci => createOrderDto.ProductIds.Contains(ci.ProductId)).ToList();

                if (!itemsToCheckout.Any())
                {
                    throw new InvalidOperationException("Không có sản phẩm nào được chọn để thanh toán.");
                }

                var orderItems = new List<OrderItem>();
                decimal totalAmount = 0;

                foreach (var cartItem in itemsToCheckout)
                {
                    var product = cartItem.Product;
                    if (product.StockQuantity < cartItem.Quantity)
                        throw new InvalidOperationException($"Sản phẩm '{product.Name}' không đủ hàng trong kho.");

                    product.StockQuantity -= cartItem.Quantity;
                    if (product.StockQuantity == 0) product.Status = EProductStatus.OutOfStock;

                    var orderItem = new OrderItem { ProductId = cartItem.ProductId, Quantity = cartItem.Quantity, Price = product.Price };
                    orderItems.Add(orderItem);
                    totalAmount += orderItem.Price * orderItem.Quantity;
                }

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = createOrderDto.ShippingAddress,
                    PhoneNumber = createOrderDto.PhoneNumber,
                    TotalAmount = totalAmount,
                    Status = EOrderStatus.Processing,
                    OrderItems = orderItems
                };

                await _orderRepository.CreateOrderAsync(order);
                await _orderRepository.SaveChangesAsync();

                var paymentTransaction = new PaymentTransaction
                {
                    OrderId = order.Id,
                    Amount = order.TotalAmount,
                    PaymentMethod = createOrderDto.PaymentMethod,
                    Status = EPaymentStatus.Success,
                    TransactionDate = DateTime.UtcNow,
                    TransactionId = Guid.NewGuid().ToString()
                };
                await _orderRepository.AddPaymentTransactionAsync(paymentTransaction);

                await _shoppingCartRepository.RemoveItemsAsync(userId, createOrderDto.ProductIds);

                await _orderRepository.SaveChangesAsync();
                await transaction.CommitAsync();

                var finalOrder = await _orderRepository.GetOrderByOrderIdAsync(order.Id);
                return MapOrderToDto(finalOrder!);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Phương thức lấy danh sách đơn hàng của người dùng theo ID.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <param name="query">Thông tin truy vấn để phân trang và lọc đơn hàng.</param>
        /// <returns>Trả về danh sách đơn hàng đã phân trang.</returns>
        public async Task<PagedResult<OrderDto>> GetOrdersbyUserIdAsync(long userId, OrderQuery query)
        {
            var pagedOrders = await _orderRepository.GetOrdersByUserIdAsync(userId, query);
            var orderDtos = pagedOrders.Items?.Select(MapOrderToDto).ToList();
            return new PagedResult<OrderDto> { Items = orderDtos, TotalItems = pagedOrders.TotalItems, PageNumber = pagedOrders.PageNumber, PageSize = pagedOrders.PageSize, TotalPages = pagedOrders.TotalPages };
        }

        /// <summary>
        /// Phương thức lấy chi tiết đơn hàng của người dùng theo ID đơn hàng.
        /// </summary>
        /// <param name="orderId">ID của đơn hàng cần lấy chi tiết.</param>
        /// <param name="userId">ID của người dùng để xác thực quyền truy cập.</param>
        /// <returns>Trả về chi tiết đơn hàng.</returns>
        /// <exception cref="KeyNotFoundException">Nếu không tìm thấy đơn hàng với ID hoặc người dùng không có quyền truy cập.</exception>
        public async Task<OrderDto> GetOrderDetailByOrderIdIdAsync(long orderId, long userId)
        {
            var order = await _orderRepository.GetOrderByIdAndUserIdAsync(orderId, userId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {orderId} hoặc bạn không có quyền truy cập.");
            }
            return MapOrderToDto(order);
        }

        /// <summary>
        /// Phương thức hủy đơn hàng của người dùng theo ID đơn hàng.
        /// </summary>
        /// <param name="orderId">ID của đơn hàng cần hủy.</param>
        /// <param name="userId">ID của người dùng để xác thực quyền truy cập.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Nếu không tìm thấy đơn hàng với ID hoặc người dùng không có quyền truy cập.</exception>
        /// <exception cref="InvalidOperationException">Nếu đơn hàng không ở trạng thái có thể hủy (Pending hoặc Processing).</exception>
        public async Task<OrderDto> CancelUserOrderByOrderIdAsync(long orderId, long userId)
        {
            var order = await _orderRepository.GetOrderByIdAndUserIdAsync(orderId, userId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {orderId} hoặc bạn không có quyền hủy đơn hàng này.");
            }
            if (order.Status != EOrderStatus.Pending && order.Status != EOrderStatus.Processing)
            {
                throw new InvalidOperationException($"Không thể hủy đơn hàng ở trạng thái '{order.Status}'.");
            }

            await RollbackStockForOrder(order);
            order.Status = EOrderStatus.Cancelled;
            await _orderRepository.SaveChangesAsync();
            return MapOrderToDto(order);
        }

        /// <summary>
        /// Phương thức hủy đơn hàng cho quản trị viên theo ID đơn hàng.
        /// </summary>
        /// <param name="orderId">ID của đơn hàng cần hủy.</param>
        /// <returns>Trả về đơn hàng đã được hủy.</returns>
        /// <exception cref="KeyNotFoundException">Nếu không tìm thấy đơn hàng với ID.</exception>
        /// <exception cref="InvalidOperationException">Nếu đơn hàng không ở trạng thái có thể hủy (Pending hoặc Processing).</exception>
        public async Task<OrderDto> CancelOrderForAdminAsync(long orderId)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {orderId}");
            }
            if (order.Status != EOrderStatus.Pending && order.Status != EOrderStatus.Processing)
            {
                throw new InvalidOperationException($"Không thể hủy đơn hàng ở trạng thái '{order.Status}'.");
            }

            await RollbackStockForOrder(order);
            order.Status = EOrderStatus.Cancelled;
            await _orderRepository.SaveChangesAsync();
            return MapOrderToDto(order);
        }

        /// <summary>
        /// Phương thức lấy tất cả đơn hàng cho quản trị viên.
        /// </summary>
        /// <param name="query">Thông tin truy vấn để phân trang và lọc đơn hàng.</param>
        /// <returns>Trả về danh sách đơn hàng đã phân trang.</returns>
        public async Task<PagedResult<OrderDto>> GetAllOrdersAsync(OrderQuery query)
        {
            var pagedOrders = await _orderRepository.GetAllOrdersAsync(query);
            var orderDtos = pagedOrders.Items?.Select(MapOrderToDto).ToList();
            return new PagedResult<OrderDto> { Items = orderDtos, TotalItems = pagedOrders.TotalItems, PageNumber = pagedOrders.PageNumber, PageSize = pagedOrders.PageSize, TotalPages = pagedOrders.TotalPages };
        }

        /// <summary>
        /// Phương thức lấy chi tiết đơn hàng cho quản trị viên theo ID đơn hàng.
        /// </summary>
        /// <param name="orderId">ID của đơn hàng cần lấy chi tiết.</param>
        /// <returns>Trả về chi tiết đơn hàng.</returns>
        /// <exception cref="KeyNotFoundException">Nếu không tìm thấy đơn hàng với ID.</exception>
        public async Task<OrderDto> GetOrderDetailForAdminAsync(long orderId)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {orderId}");
            }
            return MapOrderToDto(order);
        }

        /// <summary>
        /// Phương thức cập nhật trạng thái đơn hàng cho quản trị viên.
        /// </summary>
        /// <param name="orderId">ID của đơn hàng cần cập nhật.</param>
        /// <param name="newStatus">Trạng thái mới của đơn hàng.</param>
        /// <returns>Trả về đơn hàng đã được cập nhật.</returns>
        /// <exception cref="KeyNotFoundException">Nếu không tìm thấy đơn hàng với ID.</exception>
        public async Task<OrderDto> UpdateOrderStatusAsync(long orderId, EOrderStatus newStatus)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {orderId}");
            }
            order.Status = newStatus;
            await _orderRepository.UpdateAsync(order);
            return MapOrderToDto(order);
        }

        /// <summary>
        /// Phương thức hoàn lại hàng tồn kho cho đơn hàng đã hủy.
        /// </summary>
        /// <param name="order">Đơn hàng cần hoàn lại hàng tồn kho.</param>
        /// <returns>Trả về một tác vụ bất đồng bộ.</returns>
        private async Task RollbackStockForOrder(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdWithImagesAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                    if (product.Status == EProductStatus.OutOfStock && product.StockQuantity > 0)
                    {
                        product.Status = EProductStatus.Available;
                    }
                }
            }
        }

        /// <summary>
        /// Phương thức ánh xạ đơn hàng sang DTO.
        /// </summary>
        /// <param name="order">Đơn hàng cần ánh xạ.</param>
        /// <returns>Trả về DTO của đơn hàng.</returns>
        private OrderDto MapOrderToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                PhoneNumber = order.PhoneNumber,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList() ?? new List<OrderItemDto>()
            };
        }
    }
}