using System.ComponentModel.DataAnnotations;

namespace Final.WebApp.DTOs.Users
{
    public class UpdateProfileDTO
    {
        [Required(ErrorMessage = "Tên không được để trống.")]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Họ không được để trống.")]
        [StringLength(50)]
        public string LastName { get; set; } = null!;
    }
}
