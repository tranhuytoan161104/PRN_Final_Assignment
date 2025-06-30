using Final.Domain.Entities;
using Final.Domain.Enums;

namespace Final.ProductAPI.DTOs
{
    public class ProductDetailDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime AddAt { get; set; }
        public EProductStatus Status { get; set; }
        public int StockQuantity { get; set; }
        public long BrandId { get; set; }
        public long CategoryId { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }
        public List<ReviewDTO>? Reviews { get; set; }
        public List<ProductImageDTO>? Images { get; set; }
    }
}
