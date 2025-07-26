namespace Final.WebApp.DTOs.Carts
{
    public class CartDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public List<CartItemDTO> Items { get; set; } = [];
        public decimal TotalPrice { get; set; }
    }
}
