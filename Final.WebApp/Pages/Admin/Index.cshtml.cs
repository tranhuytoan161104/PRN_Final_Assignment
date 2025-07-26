using Final.WebApp.DTOs.Dashboard;
using Final.WebApp.DTOs.Users;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Final.WebApp.Pages.Admin;

[Authorize(Roles = "Admin, Owner")]
public class IndexModel : PageModel
{
    private readonly IDashboardApiService _dashboardApi;
    private readonly IUserApiService _userApi;

    public IndexModel(IDashboardApiService dashboardApi, IUserApiService userApi)
    {
        _dashboardApi = dashboardApi;
        _userApi = userApi;
    }

    public DashboardStatsDTO Stats { get; set; } = new();
    public List<RecentOrderDTO> RecentOrders { get; set; } = [];
    public List<RecentUserDTO> RecentUsers { get; set; } = [];
    public string RevenueChartJsonData { get; set; } = "[]";

    public async Task OnGetAsync()
    {
        // Gọi các API song song để tăng tốc độ tải trang
        var statsTask = _dashboardApi.GetStatsAsync();
        var revenueTask = _dashboardApi.GetRevenueChartDataAsync();
        var ordersTask = _dashboardApi.GetRecentOrdersAsync();
        var usersTask = _userApi.GetRecentUsersAsync();

        await Task.WhenAll(statsTask, revenueTask, ordersTask, usersTask);

        Stats = await statsTask;
        RecentOrders = await ordersTask;
        RecentUsers = await usersTask;

        // Chuẩn bị dữ liệu cho biểu đồ và serialize thành JSON
        var revenueData = await revenueTask;
        RevenueChartJsonData = JsonSerializer.Serialize(revenueData);
    }
}