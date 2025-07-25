using Final.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Final.OrderAPI.DTOs
{
    public class UpdateOrderStatusDTO
    {
        [Required(ErrorMessage = "Trạng thái mới không được để trống.")]
        [EnumDataType(typeof(EOrderStatus), ErrorMessage = "Trạng thái không hợp lệ.")]
        public EOrderStatus Status { get; set; }
    }
}
