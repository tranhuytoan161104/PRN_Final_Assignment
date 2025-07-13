using Final.Domain.Common;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.OrderAPI.DTOs;
using Final.OrderAPI.Services;
using Final.PaymentAPI.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace Final.PaymentAPI.Services
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IPaymentTransactionRepository _repository;

        public PaymentTransactionService(IPaymentTransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaymentTransactionDto?> GetByIdAsync(long id)
        {
            var transaction = await _repository.GetByIdAsync(id);
            return transaction == null ? null : MapToDto(transaction);
        }

        public async Task<PagedResult<PaymentTransactionDto>> GetAllAsync(PaymentTransactionQuery query)
        {
            var pagedResult = await _repository.GetAllAsync(query);
            var dtos = pagedResult.Items.Select(MapToDto).ToList();

            return new PagedResult<PaymentTransactionDto>
            {
                Items = dtos,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalItems = pagedResult.TotalItems,
                TotalPages = pagedResult.TotalPages
            };
        }

        private PaymentTransactionDto MapToDto(Final.Domain.Entities.PaymentTransaction t)
        {
            return new PaymentTransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate,
                Status = t.Status,
                PaymentMethod = t.PaymentMethod,
                TransactionId = t.TransactionId,
                OrderId = t.OrderId
            };
        }
    }
}