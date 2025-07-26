﻿using System.ComponentModel.DataAnnotations;

namespace Final.WebApp.DTOs.PasswordReset
{
    public class ResetPasswordDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; } = null!;
    }
}
