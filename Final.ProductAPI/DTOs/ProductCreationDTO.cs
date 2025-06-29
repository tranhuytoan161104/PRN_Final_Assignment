namespace Final.ProductAPI.DTOs
{
    public class ProductCreationDTO
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public long BrandId { get; set; }
        public long CategoryId { get; set; }
        public List<ReviewDTO>? Reviews { get; set; }
        public List<string>? Images { get; set; }
    }
}
