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

        public async Task<PaymentTransaction?> GetByIdAsync(long id)
        {
            return await _context.PaymentTransactions
                .Include(t => t.Order) // Lấy kèm thông tin đơn hàng
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<PagedResult<PaymentTransaction>> GetAllAsync(PaymentTransactionQuery query)
        {
            var queryable = _context.PaymentTransactions
                .Include(t => t.Order)
                .OrderByDescending(t => t.TransactionDate);

            var totalItems = await queryable.CountAsync();
            var items = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<PaymentTransaction>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)
            };
        }
    }
}
