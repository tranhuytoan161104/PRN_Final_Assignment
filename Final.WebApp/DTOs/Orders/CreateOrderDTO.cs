using System.ComponentModel.DataAnnotations;

namespace Final.WebApp.DTOs.Orders
{
    public class CreateOrderDTO
    {
        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống.")]
        [StringLength(255)]
        public string ShippingAddress { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        public string PaymentMethod { get; set; } = null!;

        public List<long> ProductIds { get; set; } = [];
    }
}
