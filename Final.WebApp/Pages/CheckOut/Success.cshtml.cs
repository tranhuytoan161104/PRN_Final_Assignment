using Final.WebApp.DTOs.Orders;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Checkout;

[Authorize]
public class SuccessModel : PageModel
{
    // Trang này có thể đơn giản không cần gọi API lại
    // Nhưng nếu muốn hiển thị thông tin chi tiết, ta sẽ gọi API
    // private readonly IOrderApiService _orderApiService;
    // public SuccessModel(IOrderApiService orderApiService) { ... }

    public long OrderId { get; set; }

    public void OnGet(long orderId)
    {
        OrderId = orderId;
    }
}