using Final.Domain.Interfaces;
using Final.OrderAPI.DTOs;

namespace Final.OrderAPI.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _repository;
        public PaymentMethodService(IPaymentMethodRepository repository) { _repository = repository; }

        public async Task<List<PaymentMethodDto>> GetActiveMethodsAsync()
        {
            var methods = await _repository.GetActiveMethodsAsync();
            return methods.Select(m => new PaymentMethodDto
            {
                Code = m.Code
            }).ToList();
        }
    }
}
