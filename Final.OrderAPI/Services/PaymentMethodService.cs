using Final.Domain.Interfaces;
using Final.OrderAPI.DTOs;

namespace Final.OrderAPI.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _repository;
        public PaymentMethodService(IPaymentMethodRepository repository) { _repository = repository; }

        public async Task<List<PaymentMethodDTO>> GetAllActiveMethodsAsync()
        {
            var methods = await _repository.GetAllActiveMethodsAsync();
            return methods.Select(m => new PaymentMethodDTO { Code = m.Code }).ToList();
        }
    }
}