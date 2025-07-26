namespace Final.OrderAPI.DTOs
{
    public class DashboardStatsDTO
    {
        public decimal MonthlyRevenue { get; set; }
        public int NewOrdersToday { get; set; }
        public int PendingOrders { get; set; }
        public int NewUsersThisMonth { get; set; }
    }
}
