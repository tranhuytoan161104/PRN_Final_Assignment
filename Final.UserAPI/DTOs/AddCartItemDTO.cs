using System.ComponentModel.DataAnnotations;

namespace Final.UserAPI.DTOs
{
    public class AddCartItemDTO
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long ProductId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Số lượng phải từ 1 đến 100.")]
        public int Quantity { get; set; }
    }
}