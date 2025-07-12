namespace Final.UserAPI.DTOs
{
    public class CartDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal TotalPrice { get; set; }
    }
}
