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

        public async Task<PaymentTransactionDto> GetByIdAsync(long id)
        {
            var transaction = await _repository.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy giao dịch với ID: {id}");
            }
            return MapToDto(transaction);
        }

        public async Task<PagedResult<PaymentTransactionDto>> GetAllAsync(PaymentTransactionQuery query)
        {
            var pagedResult = await _repository.GetAllAsync(query);
            var dtos = pagedResult.Items.Select(MapToDto).ToList();
            return new PagedResult<PaymentTransactionDto> { Items = dtos, TotalItems = pagedResult.TotalItems, PageNumber = pagedResult.PageNumber, PageSize = pagedResult.PageSize, TotalPages = pagedResult.TotalPages };
        }

        private PaymentTransactionDto MapToDto(Final.Domain.Entities.PaymentTransaction t)
        {
            return new PaymentTransactionDto { Id = t.Id, Amount = t.Amount, TransactionDate = t.TransactionDate, Status = t.Status, PaymentMethod = t.PaymentMethod, TransactionId = t.TransactionId, OrderId = t.OrderId };
        }
    }
}