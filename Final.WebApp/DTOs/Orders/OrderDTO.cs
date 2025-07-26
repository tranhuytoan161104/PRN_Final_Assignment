namespace Final.WebApp.DTOs.Orders
{
    public class OrderDTO
    {
        public long Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!; 
        public string ShippingAddress { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = [];
    }
}
