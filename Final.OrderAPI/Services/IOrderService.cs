using Final.Domain.Common;
using Final.Domain.Enums;
using Final.Domain.Queries;
using Final.OrderAPI.DTOs;

namespace Final.OrderAPI.Services
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderFromUserCartAsync(long userId, CreateOrderDTO createOrderDto);
        Task<PagedResult<OrderDTO>> GetOrdersbyUserIdAsync(long userId, OrderQuery query);
        Task<OrderDTO> GetOrderDetailByOrderIdAsync(long orderId, long userId);
        Task<OrderDTO> CancelUserOrderByOrderIdAsync(long orderId, long userId);
        Task<PagedResult<OrderDTO>> GetAllOrdersAsync(OrderQuery query);
        Task<OrderDTO> GetOrderDetailForAdminAsync(long orderId);
        Task<OrderDTO> UpdateOrderStatusAsync(long orderId, EOrderStatus newStatus);
        Task<OrderDTO> CancelOrderForAdminAsync(long orderId);
    }
}
