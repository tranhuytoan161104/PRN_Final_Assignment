using Final.WebApp.DTOs.Users;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final.WebApp.Pages.Admin.Users;

[Authorize(Roles = "Admin, Owner")]
public class EditModel : PageModel
{
    private readonly IUserApiService _userApiService;
    public EditModel(IUserApiService userApiService)
    {
        _userApiService = userApiService;
    }

    public UserProfileDTO UserProfile { get; set; } = new();

    [BindProperty]
    public UpdateUserStatusDTO StatusInput { get; set; } = new();

    [BindProperty]
    public UserRoleDTO RoleInput { get; set; } = new();


    public SelectList Statuses { get; set; } = default!;
    public SelectList Roles { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(long userId)
    {
        try
        {
            UserProfile = await _userApiService.GetUserByIdAsync(userId);
            Statuses = new SelectList(new[] { "Active", "Inactive" }, UserProfile.Status.ToString());
            Roles = new SelectList(new[] { "Owner", "Admin", "Customer" }, UserProfile.Role);
            return Page();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(long userId)
    {
        await _userApiService.UpdateUserStatusAsync(userId, StatusInput);
        TempData["SuccessMessage"] = "Cập nhật trạng thái thành công!";
        return RedirectToPage(new { userId });
    }

    [Authorize(Roles = "Owner")] // Chỉ Owner mới được gọi handler này
    public async Task<IActionResult> OnPostUpdateRoleAsync(long userId)
    {
        await _userApiService.UpdateUserRoleAsync(userId, RoleInput);
        TempData["SuccessMessage"] = "Cập nhật vai trò thành công!";
        return RedirectToPage(new { userId });
    }
}