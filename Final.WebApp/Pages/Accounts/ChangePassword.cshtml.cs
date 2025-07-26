using Final.WebApp.DTOs.Users;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Accounts
{

    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserApiService _userApiService;
        public ChangePasswordModel(IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        [BindProperty]
        public ChangePasswordDTO PasswordInput { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _userApiService.ChangeMyPasswordAsync(PasswordInput);
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToPage("/Accounts/Profile");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}