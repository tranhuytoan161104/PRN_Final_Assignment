namespace Final.OrderAPI.DTOs
{
    public class CreateOrderDto
    {
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public List<long> ProductIds { get; set; }
        public string PaymentMethod { get; set; }
    }
}
