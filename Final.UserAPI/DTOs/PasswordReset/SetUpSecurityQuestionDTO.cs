using System.ComponentModel.DataAnnotations;

namespace Final.UserAPI.DTOs.PasswordReset
{
    public class SetupSecurityQuestionDTO
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public string Question { get; set; } = null!;

        [Required]
        public string Answer { get; set; } = null!;
    }
}
