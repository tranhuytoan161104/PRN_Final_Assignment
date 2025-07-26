using Final.WebApp.DTOs.Orders;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Checkout;

[Authorize]
public class SuccessModel : PageModel
{
    public long OrderId { get; set; }

    public void OnGet(long orderId)
    {
        OrderId = orderId;
    }
}