using Final.WebApp.DTOs.Users;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Accounts
{

    public class RegisterModel : PageModel
    {
        private readonly IUserApiService _userApiService;

        public RegisterModel(IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        [BindProperty]
        public RegisterDTO RegisterInput { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _userApiService.RegisterAsync(RegisterInput);

                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Vui lòng đăng nhập.";

                return RedirectToPage("/Accounts/Login");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Đã có lỗi không mong muốn xảy ra. Xin vui lòng thử lại.");
                return Page();
            }
        }
    }
}