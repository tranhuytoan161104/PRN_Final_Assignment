using System.ComponentModel.DataAnnotations;

namespace Final.ProductAPI.DTOs
{
    public class UpdateStockQuantityDTO
    {
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int NewStockQuantity { get; set; }
    }
}