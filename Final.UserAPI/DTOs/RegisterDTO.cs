using System.ComponentModel.DataAnnotations;

namespace Final.UserAPI.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Tên không được để trống.")]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Họ không được để trống.")]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi @gmail.com.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Mật khẩu phải chứa ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}