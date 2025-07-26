using Final.WebApp.Services;
using Final.WebApp.DTOs.PasswordReset;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Accounts
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IUserApiService _userApiService;
        public ResetPasswordModel(IUserApiService userApiService) { _userApiService = userApiService; }

        [BindProperty]
        public ResetPasswordDTO Input { get; set; } = new();

        public IActionResult OnGet(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Thiếu thông tin để đặt lại mật khẩu.");
            }
            Input = new ResetPasswordDTO { Email = email, Token = token };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _userApiService.ResetPasswordAsync(Input);
                TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công. Vui lòng đăng nhập lại.";
                return RedirectToPage("/Accounts/Login");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
