using Final.Domain.Enums;

namespace Final.OrderAPI.DTOs
{
    public class OrderDto
    {
        public long Id { get; set; }
        public DateTime OrderDate { get; set; }
        public EOrderStatus Status { get; set; }
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
