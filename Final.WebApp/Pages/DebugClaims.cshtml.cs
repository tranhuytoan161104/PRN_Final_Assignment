using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages;

[Authorize]
public class DebugClaimsModel : PageModel
{
    public void OnGet()
    {
    }
}