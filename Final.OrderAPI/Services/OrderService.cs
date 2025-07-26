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

        public async Task<OrderDTO> CreateOrderFromUserCartAsync(long userId, CreateOrderDTO createOrderDto)
        {
            await using var transaction = await _orderRepository.BeginTransactionAsync();
            try
            {
                var cart = await _shoppingCartRepository.GetOrCreateCartForUserAsync(userId);
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
                await _orderRepository.UpdateOrderAsync();

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

                await _shoppingCartRepository.RemoveItemsFromUserCartAsync(userId, createOrderDto.ProductIds);

                await _orderRepository.UpdateOrderAsync();
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

        public async Task<PagedResult<OrderDTO>> GetOrdersbyUserIdAsync(long userId, OrderQuery query)
        {
            var pagedOrders = await _orderRepository.GetOrdersByUserIdAsync(userId, query);
            var orderDtos = pagedOrders.Items?.Select(MapOrderToDto).ToList();
            return new PagedResult<OrderDTO> { Items = orderDtos, TotalItems = pagedOrders.TotalItems, PageNumber = pagedOrders.PageNumber, PageSize = pagedOrders.PageSize, TotalPages = pagedOrders.TotalPages };
        }

        public async Task<OrderDTO> GetOrderDetailByOrderIdAsync(long orderId, long userId)
        {
            var order = await _orderRepository.GetOrderByOrderIdAndUserIdAsync(orderId, userId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {orderId} hoặc bạn không có quyền truy cập.");
            }
            return MapOrderToDto(order);
        }

        public async Task<OrderDTO> CancelUserOrderByOrderIdAsync(long orderId, long userId)
        {
            var order = await _orderRepository.GetOrderByOrderIdAndUserIdAsync(orderId, userId);
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
            await _orderRepository.UpdateOrderAsync();
            return MapOrderToDto(order);
        }

        public async Task<OrderDTO> CancelOrderForAdminAsync(long orderId)
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
            await _orderRepository.UpdateOrderAsync();
            return MapOrderToDto(order);
        }

        public async Task<PagedResult<OrderDTO>> GetAllOrdersAsync(OrderQuery query)
        {
            var pagedOrders = await _orderRepository.GetAllOrdersAsync(query);
            var orderDtos = pagedOrders.Items?.Select(MapOrderToDto).ToList();
            return new PagedResult<OrderDTO> { Items = orderDtos, TotalItems = pagedOrders.TotalItems, PageNumber = pagedOrders.PageNumber, PageSize = pagedOrders.PageSize, TotalPages = pagedOrders.TotalPages };
        }

        public async Task<OrderDTO> GetOrderDetailForAdminAsync(long orderId)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID: {orderId}");
            }
            return MapOrderToDto(order);
        }

        public async Task<OrderDTO> UpdateOrderStatusAsync(long orderId, EOrderStatus newStatus)
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

        private async Task RollbackStockForOrder(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetProductByProductIdWithImagesAsync(item.ProductId);
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

        private OrderDTO MapOrderToDto(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                PhoneNumber = order.PhoneNumber,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList() ?? new List<OrderItemDTO>()
            };
        }
    }
}