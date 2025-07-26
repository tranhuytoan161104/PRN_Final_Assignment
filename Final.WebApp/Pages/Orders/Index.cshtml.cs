using Final.WebApp.DTOs.Orders;
using Final.WebApp.DTOs.Common;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Orders;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IOrderApiService _orderApiService;
    public IndexModel(IOrderApiService orderApiService)
    {
        _orderApiService = orderApiService;
    }

    public PagedResult<OrderDTO> Orders { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int p { get; set; } = 1; 

    public async Task OnGetAsync()
    {
        var query = new OrderQuery { PageNumber = p };
        Orders = await _orderApiService.GetMyOrdersAsync(query);
    }
}