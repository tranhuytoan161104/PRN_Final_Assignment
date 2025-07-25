using Final.Domain.Common;
using Final.Domain.Queries;
using Final.OrderAPI.DTOs;

namespace Final.OrderAPI.Services
{
    public interface IPaymentTransactionService
    {
        Task<PaymentTransactionDTO?> GetTransactionByTransactionIdAsync(long id);
        Task<PagedResult<PaymentTransactionDTO>> GetAllTransactionsAsync(PaymentTransactionQuery queries);
    }
}
