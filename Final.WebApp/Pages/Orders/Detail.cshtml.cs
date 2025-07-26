using Final.WebApp.DTOs.Orders;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Orders;

[Authorize]
public class DetailModel : PageModel
{
    private readonly IOrderApiService _orderApiService;
    public DetailModel(IOrderApiService orderApiService)
    {
        _orderApiService = orderApiService;
    }

    public OrderDTO Order { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(long orderId)
    {
        try
        {
            Order = await _orderApiService.GetOrderByIdAsync(orderId);
            return Page();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostCancelOrderAsync(long orderId)
    {
        try
        {
            await _orderApiService.CancelOrderAsync(orderId);
            TempData["SuccessMessage"] = "Đã hủy đơn hàng thành công.";
        }
        catch (HttpRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToPage(new { orderId });
    }
}