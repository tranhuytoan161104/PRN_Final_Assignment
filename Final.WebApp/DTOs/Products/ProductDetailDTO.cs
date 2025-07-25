namespace Final.WebApp.DTOs.Products
{
    public class ProductDetailDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }
        public List<ProductReviewDTO> Reviews { get; set; } = [];
        public List<ProductImageDTO> Images { get; set; } = [];
    }
}
