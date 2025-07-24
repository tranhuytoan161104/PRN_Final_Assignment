using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Queries;
using Microsoft.EntityFrameworkCore.Storage;

namespace Final.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<PagedResult<Order>> GetOrdersByUserIdAsync(long userId, OrderQuery query);
        Task<Order?> GetOrderByIdAndUserIdAsync(long orderId, long userId);
        Task UpdateAsync(Order order);
        Task<PagedResult<Order>> GetAllOrdersAsync(OrderQuery query);
        Task<Order?> GetByIdAsync(long orderId);

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task AddPaymentTransactionAsync(PaymentTransaction transaction);
        Task<int> SaveChangesAsync();
    }
}