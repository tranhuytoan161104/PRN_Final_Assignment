namespace Final.WebApp.DTOs.Dashboard
{
    public class RecentOrderDTO
    {
        public long Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime OrderDate { get; set; }
    }
}
