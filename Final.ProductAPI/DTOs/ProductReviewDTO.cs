namespace Final.ProductAPI.DTOs
{
    public class ProductReviewDTO
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserName { get; set; }
    }
}
