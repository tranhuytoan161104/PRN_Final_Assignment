using System.ComponentModel.DataAnnotations;

namespace Final.UserAPI.DTOs.PasswordReset
{
    public class LinkRecoveryEmailDTO
    {
        [Required, EmailAddress]
        public string RecoveryEmail { get; set; } = null!;

        [Required] 
        public string VerificationUrl { get; set; } = null!;
    }
}
