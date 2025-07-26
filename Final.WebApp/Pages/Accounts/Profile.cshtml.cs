using Final.WebApp.DTOs.Users;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Accounts;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly IUserApiService _userApiService;
    public ProfileModel(IUserApiService userApiService)
    {
        _userApiService = userApiService;
    }

    [BindProperty]
    public UpdateProfileDTO ProfileInput { get; set; } = new();

    public UserProfileDTO UserProfile { get; set; } = new();


    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            UserProfile = await _userApiService.GetMyProfileAsync();
            ProfileInput = new UpdateProfileDTO
            {
                FirstName = UserProfile.FirstName,
                LastName = UserProfile.LastName
            };
            return Page();
        }
        catch (HttpRequestException)
        {
            return RedirectToPage("/Accounts/Login");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }
        try
        {
            await _userApiService.UpdateMyProfileAsync(ProfileInput);
            TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
        }
        catch (HttpRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToPage();
    }
}