using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Enums;
using Final.Domain.Interfaces;
using Final.OrderAPI.DTOs;
using Final.Persistence.Data;
using Final.Domain.Queries;
using Final.Persistence.Repositories;

namespace Final.OrderAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderService(ApplicationDbContext context, IProductRepository productRepository, IShoppingCartRepository shoppingCartRepository, IOrderRepository orderRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> CreateOrderFromCartAsync(long userId, CreateOrderDto createOrderDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Lấy giỏ hàng của user
                var cart = await _shoppingCartRepository.GetOrCreateCartByUserIdAsync(userId);

                // 2. LỌC ra những sản phẩm người dùng muốn thanh toán
                var itemsToCheckout = cart.Items
                    .Where(cartItem => createOrderDto.ProductIds.Contains(cartItem.ProductId))
                    .ToList();

                if (!itemsToCheckout.Any())
                {
                    throw new InvalidOperationException("Không có sản phẩm nào được chọn để thanh toán hoặc sản phẩm không có trong giỏ hàng.");
                }

                var orderItems = new List<OrderItem>();
                decimal totalAmount = 0;

                // 3. Xử lý logic trên các sản phẩm ĐÃ ĐƯỢC LỌC
                foreach (var cartItem in itemsToCheckout)
                {
                    // Do cart đã được include product, ta không cần gọi lại DB
                    var product = cartItem.Product;
                    if (product.StockQuantity < cartItem.Quantity)
                        throw new InvalidOperationException($"Sản phẩm '{product.Name}' không đủ hàng.");

                    product.StockQuantity -= cartItem.Quantity;

                    var orderItem = new OrderItem { ProductId = cartItem.ProductId, Quantity = cartItem.Quantity, Price = product.Price };
                    orderItems.Add(orderItem);
                    totalAmount += orderItem.Price * orderItem.Quantity;
                }

                // 4. Tạo đơn hàng như cũ
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = createOrderDto.ShippingAddress,
                    PhoneNumber = createOrderDto.PhoneNumber,
                    TotalAmount = totalAmount,
                    Status = EOrderStatus.Pending,
                    OrderItems = orderItems
                };

                _context.Orders.Add(order);

                // 5. CHỈ XÓA những sản phẩm đã được mua khỏi giỏ hàng
                await _shoppingCartRepository.RemoveItemsAsync(userId, createOrderDto.ProductIds);

                await _context.SaveChangesAsync();
                
                var paymentTransaction = new PaymentTransaction
                {
                    OrderId = order.Id,
                    Amount = order.TotalAmount,
                    PaymentMethod = createOrderDto.PaymentMethod,
                    Status = EPaymentStatus.Success,    // Giả định thanh toán mô phỏng luôn thành công
                    TransactionDate = DateTime.UtcNow,
                    TransactionId = Guid.NewGuid().ToString() // Tạo một mã giao dịch giả
                };
                _context.PaymentTransactions.Add(paymentTransaction);

                // 6. Cập nhật trạng thái đơn hàng sau khi có giao dịch thành công
                order.Status = EOrderStatus.Processing;

                // 7. Xóa những sản phẩm đã mua khỏi giỏ hàng
                await _shoppingCartRepository.RemoveItemsAsync(userId, createOrderDto.ProductIds);

                // 8. Lưu tất cả thay đổi lần cuối và commit transaction
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapOrderToDto(order);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PagedResult<OrderDto>> GetUserOrdersAsync(long userId, OrderQuery query)
        {
            var pagedOrders = await _orderRepository.GetOrdersByUserIdAsync(userId, query);

            // Ánh xạ PagedResult<Order> sang PagedResult<OrderDto>
            var orderDtos = pagedOrders.Items.Select(MapOrderToDto).ToList();

            return new PagedResult<OrderDto>
            {
                Items = orderDtos,
                PageNumber = pagedOrders.PageNumber,
                PageSize = pagedOrders.PageSize,
                TotalItems = pagedOrders.TotalItems,
                TotalPages = pagedOrders.TotalPages
            };
        }

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
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name, // Cần include Product trong Order để có Name
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }

        public async Task<OrderDto?> GetUserOrderDetailAsync(long orderId, long userId)
        {
            var order = await _orderRepository.GetOrderByIdAndUserIdAsync(orderId, userId);

            if (order == null)
            {
                // Không tìm thấy đơn hàng hoặc đơn hàng không thuộc về người dùng này
                return null;
            }

            return MapOrderToDto(order); // Tái sử dụng phương thức ánh xạ đã có
        }

        public async Task<OrderDto> CancelUserOrderAsync(long orderId, long userId)
        {
            // 1. Lấy đơn hàng của chính user đó
            var order = await _orderRepository.GetOrderByIdAndUserIdAsync(orderId, userId);
            if (order == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đơn hàng hoặc bạn không có quyền truy cập.");
            }

            // 2. QUY TẮC NGHIỆP VỤ QUAN TRỌNG
            if (order.Status != EOrderStatus.Pending)
            {
                throw new InvalidOperationException($"Không thể hủy đơn hàng ở trạng thái '{order.Status}'.");
            }

            // 3. Cập nhật trạng thái và lưu
            order.Status = EOrderStatus.Cancelled;
            await _orderRepository.UpdateAsync(order);

            return MapOrderToDto(order);
        }

        public async Task<PagedResult<OrderDto>> GetAllOrdersAsync(OrderQuery query)
        {
            var pagedOrders = await _orderRepository.GetAllOrdersAsync(query);

            // Tái sử dụng logic ánh xạ
            var orderDtos = pagedOrders.Items.Select(MapOrderToDto).ToList();

            return new PagedResult<OrderDto>
            {
                Items = orderDtos,
                PageNumber = pagedOrders.PageNumber,
                PageSize = pagedOrders.PageSize,
                TotalItems = pagedOrders.TotalItems,
                TotalPages = pagedOrders.TotalPages
            };
        }

        public async Task<OrderDto?> GetOrderDetailForAdminAsync(long orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
            {
                return null;
            }

            return MapOrderToDto(order);
        }

        public async Task<OrderDto?> UpdateOrderStatusAsync(long orderId, EOrderStatus newStatus)
        {
            // 1. Lấy đơn hàng bằng ID
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return null; // Không tìm thấy đơn hàng
            }

            // 2. Logic nghiệp vụ (có thể thêm sau)
            // Ví dụ: Không cho phép chuyển trạng thái từ Cancelled sang Processing
            // if (order.Status == EOrderStatus.Cancelled) { ... }

            // 3. Cập nhật trạng thái và lưu
            order.Status = newStatus;
            await _orderRepository.UpdateAsync(order);

            return MapOrderToDto(order);
        }
    }
}
