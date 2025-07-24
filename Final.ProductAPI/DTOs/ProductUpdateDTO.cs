using System.ComponentModel.DataAnnotations;

namespace Final.ProductAPI.DTOs
{
    public class ProductUpdateDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "ID của thương hiệu không được để trống.")]
        public long BrandId { get; set; }

        [Required(ErrorMessage = "ID của danh mục không được để trống.")]
        public long CategoryId { get; set; }

        public List<string>? Images { get; set; }
    }
}
