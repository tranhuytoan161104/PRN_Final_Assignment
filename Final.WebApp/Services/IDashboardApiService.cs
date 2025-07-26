using Final.WebApp.DTOs.Dashboard;

namespace Final.WebApp.Services
{
    public interface IDashboardApiService
    {
        Task<DashboardStatsDTO> GetStatsAsync();
        Task<List<RevenueByDateDTO>> GetRevenueChartDataAsync();
        Task<List<RecentOrderDTO>> GetRecentOrdersAsync();
    }
}
