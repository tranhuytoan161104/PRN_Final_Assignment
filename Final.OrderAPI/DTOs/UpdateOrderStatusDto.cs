using Final.Domain.Enums;

namespace Final.OrderAPI.DTOs
{
    public class UpdateOrderStatusDto
    {
        public EOrderStatus Status { get; set; }
    }
}
