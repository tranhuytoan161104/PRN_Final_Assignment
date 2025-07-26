using System.ComponentModel.DataAnnotations;

namespace Final.WebApp.DTOs.Users
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Mật khẩu cũ không được để trống.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu mới không được để trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật khẩu xác nhận không khớp.")]
        [Display(Name = "Xác nhận mật khẩu mới")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
