using Final.WebApp.DTOs.Carts;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Cart;

[Authorize] 
public class IndexModel : PageModel
{
    private readonly ICartApiService _cartApiService;
    public IndexModel(ICartApiService cartApiService) { _cartApiService = cartApiService; }

    public CartDTO Cart { get; set; } = new();

    public async Task OnGetAsync()
    {
        Cart = await _cartApiService.GetCartAsync();
    }

    public async Task<IActionResult> OnPostUpdateQuantityAsync(long productId, int quantity)
    {
        if (quantity <= 0)
        {
            return await OnPostRemoveItemAsync(productId);
        }
        try
        {
            await _cartApiService.UpdateItemQuantityAsync(productId, quantity);
            TempData["SuccessMessage"] = "Cập nhật giỏ hàng thành công.";
        }
        catch (HttpRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message; 
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveItemAsync(long productId)
    {
        await _cartApiService.RemoveItemFromCartAsync(productId);
        TempData["SuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
        return RedirectToPage();
    }
}