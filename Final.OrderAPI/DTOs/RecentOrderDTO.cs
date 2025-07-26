using Final.Domain.Enums;

namespace Final.OrderAPI.DTOs
{
    public class RecentOrderDTO
    {
        public long Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public EOrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
