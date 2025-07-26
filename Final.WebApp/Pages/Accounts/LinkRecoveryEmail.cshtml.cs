using Final.WebApp.DTOs.PasswordReset;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Accounts;

[Authorize]
public class LinkRecoveryEmailModel : PageModel
{
    private readonly IUserApiService _userApiService;
    private readonly IConfiguration _configuration;

    public LinkRecoveryEmailModel(IUserApiService userApiService, IConfiguration configuration)
    {
        _userApiService = userApiService;
        _configuration = configuration;
    }

    [BindProperty]
    public LinkRecoveryEmailDTO RecoveryEmailInput { get; set; } = new LinkRecoveryEmailDTO();

    public void OnGet()
    {
        // Đảm bảo DTO được khởi tạo
        if (RecoveryEmailInput == null)
        {
            RecoveryEmailInput = new LinkRecoveryEmailDTO();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Console.WriteLine($"RecoveryEmail value: '{RecoveryEmailInput?.RecoveryEmail}'");
        Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

        if (!ModelState.IsValid)
        {
            // In ra tất cả lỗi validation
            foreach (var modelState in ModelState)
            {
                var key = modelState.Key;
                var errors = modelState.Value.Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"Lỗi - Key: {key}, Message: {error.ErrorMessage}");
                }
            }
            return Page();
        }

        try
        {
            var userApiUrl = _configuration["ApiSettings:UserApiUrl"];
            var verificationUrl = $"{userApiUrl}/api/users/verify-recovery-email";
            RecoveryEmailInput.VerificationUrl = verificationUrl;

            await _userApiService.LinkRecoveryEmailAsync(RecoveryEmailInput);
            TempData["SuccessMessage"] = "Một email xác thực đã được gửi đến địa chỉ mới của bạn. Vui lòng kiểm tra hộp thư.";
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }

        return RedirectToPage();
    }
}