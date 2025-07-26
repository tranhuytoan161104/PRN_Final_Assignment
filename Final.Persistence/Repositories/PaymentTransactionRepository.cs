using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Persistence.Repositories
{
    public class PaymentTransactionRepository : IPaymentTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentTransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentTransaction?> GetTransactionByTransactionIdAsync(long transactionId)
        {
            return await _context.PaymentTransactions
                .Include(t => t.Order) 
                .FirstOrDefaultAsync(t => t.Id == transactionId);
        }

        public async Task<PagedResult<PaymentTransaction>> GetAllTransactionsAsync(PaymentTransactionQuery queries)
        {
            var queryable = _context.PaymentTransactions
                .Include(t => t.Order)
                .OrderByDescending(t => t.TransactionDate);

            var totalItems = await queryable.CountAsync();
            var items = await queryable
                .Skip((queries.PageNumber - 1) * queries.PageSize)
                .Take(queries.PageSize)
                .ToListAsync();

            return new PagedResult<PaymentTransaction>
            {
                Items = items,
                PageNumber = queries.PageNumber,
                PageSize = queries.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)queries.PageSize)
            };
        }
    }
}
