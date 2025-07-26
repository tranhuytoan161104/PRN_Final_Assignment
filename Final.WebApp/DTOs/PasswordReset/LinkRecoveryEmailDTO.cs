using System.ComponentModel.DataAnnotations;

namespace Final.WebApp.DTOs.PasswordReset
{
    public class LinkRecoveryEmailDTO
    {
        [Required, EmailAddress]
        public string RecoveryEmail { get; set; } = null!;

        public string? VerificationUrl { get; set; }
    }
}
