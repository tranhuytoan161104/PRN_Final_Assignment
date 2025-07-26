using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Enums;
using Final.Domain.Queries;
using Microsoft.EntityFrameworkCore.Storage;

namespace Final.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<PagedResult<Order>> GetOrdersByUserIdAsync(long userId, OrderQuery query);
        Task<Order?> GetOrderByOrderIdAndUserIdAsync(long orderId, long userId);
        Task UpdateAsync(Order order);
        Task<PagedResult<Order>> GetAllOrdersAsync(OrderQuery query);
        Task<Order?> GetOrderByOrderIdAsync(long orderId);

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task AddPaymentTransactionAsync(PaymentTransaction transaction);
        Task<int> UpdateOrderAsync();

        Task<decimal> GetTotalRevenueInDateRange(DateTime startDate, DateTime endDate);
        Task<int> CountOrdersInDateRange(DateTime startDate, DateTime endDate);
        Task<int> CountOrdersByStatus(EOrderStatus status);
        Task<List<Order>> GetRecentOrdersAsync(int count); 
        Task<List<RevenueData>> GetRevenueGroupedByDate(DateTime startDate, DateTime endDate);
    }
}