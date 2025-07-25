using System.ComponentModel.DataAnnotations;

namespace Final.OrderAPI.DTOs
{
    public class CreateOrderDTO
    {
        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống.")]
        [StringLength(255, ErrorMessage = "Địa chỉ giao hàng không được vượt quá 255 ký tự.")]
        public string ShippingAddress { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn ít nhất một sản phẩm để thanh toán.")]
        [MinLength(1, ErrorMessage = "Vui lòng chọn ít nhất một sản phẩm để thanh toán.")]
        public List<long> ProductIds { get; set; } = new List<long>();

        [Required(ErrorMessage = "Phương thức thanh toán không được để trống.")]
        public string PaymentMethod { get; set; } = null!;
    }
}
