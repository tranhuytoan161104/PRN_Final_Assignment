using Final.OrderAPI.DTOs;

namespace Final.OrderAPI.Services
{
    public interface IPaymentMethodService
    {
        Task<List<PaymentMethodDTO>> GetAllActiveMethodsAsync();
    }
}
