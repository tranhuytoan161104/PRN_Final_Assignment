using Final.Domain.Enums;
using Final.Domain.Interfaces;
using Final.OrderAPI.DTOs;

namespace Final.OrderAPI.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public DashboardService(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public async Task<DashboardStatsDTO> GetStatsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var monthlyRevenue = await _orderRepository.GetTotalRevenueInDateRange(startOfMonth, today.AddDays(1));
            var newOrdersToday = await _orderRepository.CountOrdersInDateRange(today, today.AddDays(1));
            var pendingOrders = await _orderRepository.CountOrdersByStatus(EOrderStatus.Processing);
            var newUsersThisMonth = await _userRepository.CountUsersInDateRange(startOfMonth, today.AddDays(1));

            return new DashboardStatsDTO
            {
                MonthlyRevenue = monthlyRevenue,
                NewOrdersToday = newOrdersToday,
                PendingOrders = pendingOrders,
                NewUsersThisMonth = newUsersThisMonth
            };
        }

        public async Task<List<RevenueByDateDTO>> GetRevenueChartDataAsync()
        {
            var today = DateTime.UtcNow.Date;
            var startDate = today.AddDays(-29);

            // Lấy dữ liệu doanh thu thực tế từ DB
            var revenueDataFromDb = await _orderRepository.GetRevenueGroupedByDate(startDate, today.AddDays(1));

            // Chuyển đổi sang Dictionary để tra cứu nhanh
            var revenueDict = revenueDataFromDb.ToDictionary(r => r.Date.ToString("yyyy-MM-dd"), r => r.Revenue);

            var fullRevenueData = new List<RevenueByDateDTO>();

            // Tạo dữ liệu cho đủ 30 ngày
            for (int i = 0; i < 30; i++)
            {
                var date = startDate.AddDays(i);
                var dateString = date.ToString("yyyy-MM-dd");

                fullRevenueData.Add(new RevenueByDateDTO
                {
                    Date = dateString,
                    // Nếu có doanh thu trong DB thì lấy, không thì mặc định là 0
                    Revenue = revenueDict.TryGetValue(dateString, out var revenue) ? revenue : 0
                });
            }

            return fullRevenueData;
        }

        public async Task<List<RecentOrderDTO>> GetRecentOrdersAsync()
        {
            var recentOrders = await _orderRepository.GetRecentOrdersAsync(5);

            // Mapping từ Order (Entity) sang RecentOrderDTO (API)
            return recentOrders.Select(o => new RecentOrderDTO
            {
                Id = o.Id,
                CustomerName = o.User != null ? $"{o.User.FirstName} {o.User.LastName}" : "N/A",
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                OrderDate = o.OrderDate
            }).ToList();
        }
    }
}
