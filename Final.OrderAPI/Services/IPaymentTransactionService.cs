using Final.Domain.Common;
using Final.Domain.Queries;
using Final.OrderAPI.DTOs;

namespace Final.OrderAPI.Services
{
    public interface IPaymentTransactionService
    {
        Task<PaymentTransactionDto?> GetByIdAsync(long id);
        Task<PagedResult<PaymentTransactionDto>> GetAllAsync(PaymentTransactionQuery query);
    }
}
