using Final.WebApp.DTOs.Dashboard;

namespace Final.WebApp.Services
{
    public class DashboardApiService : IDashboardApiService
    {
        private readonly HttpClient _httpClient;
        public DashboardApiService(HttpClient httpClient) { _httpClient = httpClient; }

        public async Task<DashboardStatsDTO> GetStatsAsync()
        {
            return await _httpClient.GetFromJsonAsync<DashboardStatsDTO>("api/dashboard/stats") ?? new();
        }

        public async Task<List<RevenueByDateDTO>> GetRevenueChartDataAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RevenueByDateDTO>>("api/dashboard/revenue-chart") ?? [];
        }

        public async Task<List<RecentOrderDTO>> GetRecentOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RecentOrderDTO>>("api/dashboard/recent-orders") ?? [];
        }
    }
}
