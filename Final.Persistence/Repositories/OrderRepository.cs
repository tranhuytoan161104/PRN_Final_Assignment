using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) { _context = context; }
        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            return order;
        }

        /// <summary>
        /// Lấy danh sách đơn hàng của người dùng theo ID.
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <param name="query">Thông tin truy vấn để phân trang và lọc đơn hàng</param>
        /// <returns></returns>
        public async Task<PagedResult<Order>> GetOrdersByUserIdAsync(long userId, OrderQuery query)
        {
            var queryable = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId) 
                .OrderByDescending(o => o.OrderDate); 

            var totalItems = await queryable.CountAsync();
            var items = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Order>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)
            };
        }

        /// <summary>
        /// Lấy đơn hàng theo ID và ID người dùng.
        /// </summary>
        /// <param name="orderId">ID của đơn hàng</param>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Trả về đơn hàng nếu tìm thấy, ngược lại trả về null</returns>
        public async Task<Order?> GetOrderByOrderIdAndUserIdAsync(long orderId, long userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        /// <summary>
        /// Cập nhật thông tin đơn hàng.
        /// </summary>
        /// <param name="order">Đơn hàng cần cập nhật</param>
        /// <returns>Trả về Task khi cập nhật hoàn tất</returns>
        public async Task UpdateAsync(Order order)
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Lấy tất cả đơn hàng với phân trang.
        /// </summary>
        /// <param name="query">Thông tin truy vấn để phân trang và lọc đơn hàng</param>
        /// <returns>Trả về danh sách đơn hàng đã phân trang</returns>
        public async Task<PagedResult<Order>> GetAllOrdersAsync(OrderQuery query)
        {
            var queryable = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User) 
                .OrderByDescending(o => o.OrderDate); 

            var totalItems = await queryable.CountAsync();
            var items = await queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Order>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)
            };
        }

        /// <summary>
        /// Lấy đơn hàng theo ID đơn hàng.
        /// </summary>
        /// <param name="orderId">ID của đơn hàng</param>
        /// <returns>Trả về đơn hàng nếu tìm thấy, ngược lại trả về null</returns>
        public async Task<Order?> GetOrderByOrderIdAsync(long orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        /// <summary>
        /// Bắt đầu một giao dịch cơ sở dữ liệu.
        /// </summary>
        /// <returns>Trả về một đối tượng giao dịch cơ sở dữ liệu</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Thêm một giao dịch thanh toán vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="transaction">Giao dịch thanh toán cần thêm</param>
        /// <returns>Trả về Task khi thêm giao dịch hoàn tất</returns>
        public async Task AddPaymentTransactionAsync(PaymentTransaction transaction)
        {
            await _context.PaymentTransactions.AddAsync(transaction);
        }

        /// <summary>
        /// Lưu các thay đổi vào cơ sở dữ liệu.
        /// </summary>
        /// <returns>Trả về số lượng bản ghi đã được lưu</returns>
        public async Task<int> UpdateOrderAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
