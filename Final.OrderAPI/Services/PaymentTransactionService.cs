using Final.Domain.Common;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.OrderAPI.DTOs;
using Final.OrderAPI.Services;

namespace Final.PaymentAPI.Services
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IPaymentTransactionRepository _repository;
        public PaymentTransactionService(IPaymentTransactionRepository repository) { _repository = repository; }

        public async Task<PaymentTransactionDTO?> GetTransactionByTransactionIdAsync(long id)
        {
            var transaction = await _repository.GetTransactionByTransactionIdAsync(id);
            if (transaction == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy giao dịch với ID: {id}");
            }
            return MapToDto(transaction);
        }

        public async Task<PagedResult<PaymentTransactionDTO>> GetAllTransactionsAsync(PaymentTransactionQuery queries)
        {
            var pagedResult = await _repository.GetAllTransactionsAsync(queries);
            var dtos = pagedResult.Items.Select(MapToDto).ToList();
            return new PagedResult<PaymentTransactionDTO> { Items = dtos, TotalItems = pagedResult.TotalItems, PageNumber = pagedResult.PageNumber, PageSize = pagedResult.PageSize, TotalPages = pagedResult.TotalPages };
        }

        private PaymentTransactionDTO MapToDto(Final.Domain.Entities.PaymentTransaction t)
        {
            return new PaymentTransactionDTO { Id = t.Id, Amount = t.Amount, TransactionDate = t.TransactionDate, Status = t.Status, PaymentMethod = t.PaymentMethod, TransactionId = t.TransactionId, OrderId = t.OrderId };
        }
    }
}