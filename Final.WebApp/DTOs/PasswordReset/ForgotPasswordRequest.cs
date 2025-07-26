using System.ComponentModel.DataAnnotations;

namespace Final.WebApp.DTOs.PasswordReset
{
    public class ForgotPasswordRequestDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
    }
}
