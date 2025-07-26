namespace Final.WebApp.DTOs.Orders
{
    public class OrderItemDTO
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
