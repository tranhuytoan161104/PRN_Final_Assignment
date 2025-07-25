using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Persistence.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly ApplicationDbContext _context;
        public PaymentMethodRepository(ApplicationDbContext context) { _context = context; }

        /// <summary>
        /// Lấy các phương thức thanh toán đang hoạt động.
        /// </summary>
        /// <returns>Danh sách các phương thức thanh toán đang hoạt động.</returns>
        public async Task<List<PaymentMethod>> GetAllActiveMethodsAsync()
        {
            return await _context.PaymentMethods.Where(p => p.IsActive).ToListAsync();
        }
    }
}
