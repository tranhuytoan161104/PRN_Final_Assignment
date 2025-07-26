using Final.WebApp.DTOs.Orders;
using Final.WebApp.DTOs.Products;
using Final.WebApp.DTOs.Common;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Admin.Orders;

[Authorize(Roles = "Admin, Owner")]
public class IndexModel : PageModel
{
    private readonly IOrderApiService _orderApiService;
    public IndexModel(IOrderApiService orderApiService)
    {
        _orderApiService = orderApiService;
    }

    public PagedResult<OrderDTO> OrdersResult { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public OrderQuery Query { get; set; } = new() { PageSize = 10 };

    public async Task OnGetAsync()
    {
        OrdersResult = await _orderApiService.GetAllOrdersAsync(Query);
    }
}