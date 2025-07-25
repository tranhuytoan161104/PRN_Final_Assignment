using Final.Domain.Enums;

namespace Final.OrderAPI.DTOs
{
    public class OrderDTO
    {
        public long Id { get; set; }
        public DateTime OrderDate { get; set; }
        public EOrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
