using Final.WebApp.Services;
using Final.WebApp.DTOs.PasswordReset;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Accounts
{
    public class SetupSecurityQuestionModel : PageModel
    {
        private readonly IUserApiService _userApiService;
        public SetupSecurityQuestionModel(IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        [BindProperty]
        public SetupSecurityQuestionDTO Input { get; set; } = new();

        public IActionResult OnGet(long userId)
        {
            if (userId == 0) return RedirectToPage("/Index");
            Input.UserId = userId;
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
                await _userApiService.SetupSecurityQuestionAsync(Input);
                TempData["SuccessMessage"] = "Tạo tài khoản thành công! Vui lòng đăng nhập.";
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
