﻿using System.ComponentModel.DataAnnotations;

namespace Final.UserAPI.DTOs
{
    public class UpdateCartItemDTO
    {
        [Required]
        [Range(1, 100, ErrorMessage = "Số lượng phải từ 1 đến 100.")]
        public int Quantity { get; set; }
    }
}