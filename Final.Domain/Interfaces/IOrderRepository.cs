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
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<PagedResult<Order>> GetOrdersByUserIdAsync(long userId, OrderQuery query);
        Task<Order?> GetOrderByIdAndUserIdAsync(long orderId, long userId);
        Task UpdateAsync(Order order);
        Task<PagedResult<Order>> GetAllOrdersAsync(OrderQuery query);
        Task<Order?> GetByIdAsync(long orderId);
    }
}
