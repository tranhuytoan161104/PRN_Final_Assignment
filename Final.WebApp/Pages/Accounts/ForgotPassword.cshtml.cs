using Final.WebApp.Services;
using Final.WebApp.DTOs.PasswordReset;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Accounts
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IUserApiService _userApiService;

        public ForgotPasswordModel(IUserApiService userApiService) { _userApiService = userApiService; }

        [BindProperty(SupportsGet = true)]
        public string? Email { get; set; }

        [BindProperty]
        public string? Answer { get; set; }

        public string? SecurityQuestion { get; set; }

        public int CurrentStep { get; set; } = 1;

        public void OnGet() { CurrentStep = 1; }

        public async Task<IActionResult> OnPostEnterEmailAsync()
        {
            try
            {
                SecurityQuestion = await _userApiService.GetSecurityQuestionByEmailAsync(Email!);
                CurrentStep = 2; 
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                CurrentStep = 1;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostVerifyAnswerAsync()
        {
            try
            {
                var token = await _userApiService.VerifySecurityAnswerAndGenerateTokenAsync(
                    new VerifySecurityAnswerDTO { Email = Email!, Answer = Answer! });

                return RedirectToPage("/Accounts/ResetPassword", new { email = Email, token = token });
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("Answer", ex.Message);
                await OnPostEnterEmailAsync(); 
                return Page();
            }
        }

        public async Task<IActionResult> OnPostSendRecoveryEmailAsync()
        {
            try
            {
                var resetUrl = Url.Page("/Accounts/ResetPassword", pageHandler: null, values: null, protocol: Request.Scheme);
                await _userApiService.SendRecoveryEmailAsync(
                    new SendRecoveryEmailDTO { Email = Email!, ResetPasswordUrl = resetUrl! });

                TempData["SuccessMessage"] = "Một email khôi phục đã được gửi (nếu tài khoản của bạn đã liên kết).";
                return RedirectToPage("/Accounts/Login");
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage();
            }
        }
    }
}
