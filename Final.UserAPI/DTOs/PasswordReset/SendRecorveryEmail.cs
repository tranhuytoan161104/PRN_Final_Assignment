using System.ComponentModel.DataAnnotations;

namespace Final.UserAPI.DTOs.PasswordReset
{
    public class SendRecoveryEmailDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string ResetPasswordUrl { get; set; } = null!;
    }
}
