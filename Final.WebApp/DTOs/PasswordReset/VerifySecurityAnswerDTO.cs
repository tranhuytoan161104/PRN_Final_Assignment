using System.ComponentModel.DataAnnotations;

namespace Final.WebApp.DTOs.PasswordReset
{
    public class VerifySecurityAnswerDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Answer { get; set; } = null!;
    }
}
