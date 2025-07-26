using Final.OrderAPI.DTOs;

namespace Final.OrderAPI.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsDTO> GetStatsAsync();
        Task<List<RevenueByDateDTO>> GetRevenueChartDataAsync();
        Task<List<RecentOrderDTO>> GetRecentOrdersAsync();
    }
}
