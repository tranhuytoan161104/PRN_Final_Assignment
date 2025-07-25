using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Domain.Interfaces
{
    public interface IPaymentTransactionRepository
    {
        Task<PaymentTransaction?> GetTransactionByTransactionIdAsync(long transactionId);
        Task<PagedResult<PaymentTransaction>> GetAllTransactionsAsync(PaymentTransactionQuery queries);
    }
}
