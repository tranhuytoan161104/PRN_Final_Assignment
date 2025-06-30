using System.ComponentModel.DataAnnotations;

namespace Final.ProductAPI.DTOs
{
    public class ProductUpdateDTO
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public long BrandId { get; set; }

        public long CategoryId { get; set; }
    }
}
