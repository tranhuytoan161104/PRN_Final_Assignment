using Final.WebApp.DTOs.Orders;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final.WebApp.Pages.Admin.Orders;

[Authorize(Roles = "Admin, Owner")]
public class DetailModel : PageModel
{
    private readonly IOrderApiService _orderApiService;
    public DetailModel(IOrderApiService orderApiService)
    {
        _orderApiService = orderApiService;
    }

    public OrderDTO Order { get; set; } = new();

    [BindProperty]
    public UpdateOrderStatusDTO StatusInput { get; set; } = new();

    public SelectList OrderStatuses { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(long orderId)
    {
        try
        {
            Order = await _orderApiService.GetAdminOrderDetailAsync(orderId);
            var statuses = new[] { "Pending", "Processing", "Delivered", "Cancelled", "Failed" };
            OrderStatuses = new SelectList(statuses, Order.Status);
            return Page();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Admin")] // Chỉ Admin mới có quyền cập nhật trạng thái
    public async Task<IActionResult> OnPostUpdateStatusAsync(long orderId)
    {
        try
        {
            await _orderApiService.UpdateOrderStatusAsync(orderId, StatusInput);
            TempData["SuccessMessage"] = "Cập nhật trạng thái đơn hàng thành công!";
        }
        catch (HttpRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToPage(new { orderId });
    }
}