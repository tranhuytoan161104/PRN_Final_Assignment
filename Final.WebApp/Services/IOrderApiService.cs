using Final.WebApp.DTOs.Orders;
using Final.WebApp.DTOs.Common;

namespace Final.WebApp.Services
{
    public interface IOrderApiService
    {
        Task<List<PaymentMethodDTO>> GetPaymentMethodsAsync();
        Task<OrderDTO> CreateOrderAsync(CreateOrderDTO createOrderDto);
        Task<OrderDTO> GetOrderByIdAsync(long orderId);
        Task<PagedResult<OrderDTO>> GetMyOrdersAsync(OrderQuery query);
        Task<OrderDTO> CancelOrderAsync(long orderId);

        Task<PagedResult<OrderDTO>> GetAllOrdersAsync(OrderQuery query);
        Task<OrderDTO> GetAdminOrderDetailAsync(long orderId);
        Task UpdateOrderStatusAsync(long orderId, UpdateOrderStatusDTO dto);
    }
}
