using Final.WebApp.DTOs.Products;
using Final.WebApp.DTOs.Users;
using Final.WebApp.Services;
using Final.WebApp.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Admin.Users;

[Authorize(Roles = "Admin, Owner")]
public class IndexModel : PageModel
{
    private readonly IUserApiService _userApiService;
    public IndexModel(IUserApiService userApiService)
    {
        _userApiService = userApiService;
    }

    public PagedResult<UserDTO> UsersResult { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public UserQuery Query { get; set; } = new();

    public async Task OnGetAsync()
    {
        UsersResult = await _userApiService.GetAllUsersAsync(Query);
    }
}