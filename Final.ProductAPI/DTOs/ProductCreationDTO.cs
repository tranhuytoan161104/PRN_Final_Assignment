using System.ComponentModel.DataAnnotations;

namespace Final.ProductAPI.DTOs
{
    public class ProductCreationDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không hợp lệ.")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "ID của thương hiệu không được để trống.")]
        public long BrandId { get; set; }

        [Required(ErrorMessage = "ID của danh mục không được để trống.")]
        public long CategoryId { get; set; }

        public List<string>? Images { get; set; }
    }
}