using System.ComponentModel.DataAnnotations;

namespace Final.UserAPI.DTOs
{
    public class AddCartItemDTO
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
