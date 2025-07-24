using Final.Domain.Common;
using Final.Domain.Enums;
using Final.Domain.Queries;
using Final.OrderAPI.DTOs;

namespace Final.OrderAPI.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderFromCartAsync(long userId, CreateOrderDto createOrderDto);
        Task<PagedResult<OrderDto>> GetUserOrdersAsync(long userId, OrderQuery query);
        Task<OrderDto> GetUserOrderDetailAsync(long orderId, long userId);
        Task<OrderDto> CancelOrderForCurrentUserAsync(long orderId, long userId);
        Task<PagedResult<OrderDto>> GetAllOrdersAsync(OrderQuery query);
        Task<OrderDto> GetOrderDetailForAdminAsync(long orderId);
        Task<OrderDto> UpdateOrderStatusAsync(long orderId, EOrderStatus newStatus);
        Task<OrderDto> CancelOrderForAdminAsync(long orderId);
    }
}
