using Final.Domain.Entities;

namespace Final.ProductAPI.DTOs
{
    public class ProductDetailDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public string? CategoryName { get; set; }
        public List<ReviewDTO>? Reviews { get; set; }
    }
}
