using Final.WebApp.DTOs.Carts;
using Final.WebApp.DTOs.Orders;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final.WebApp.Pages.Checkout;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ICartApiService _cartApiService;
    private readonly IOrderApiService _orderApiService;

    public IndexModel(ICartApiService cartApiService, IOrderApiService orderApiService)
    {
        _cartApiService = cartApiService;
        _orderApiService = orderApiService;
    }

    public CartDTO Cart { get; set; } = new();
    public SelectList PaymentMethods { get; set; } = new(new List<PaymentMethodDTO>());

    [BindProperty]
    public CreateOrderDTO OrderInput { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var cartTask = _cartApiService.GetCartAsync();
        var paymentMethodsTask = _orderApiService.GetPaymentMethodsAsync();

        await Task.WhenAll(cartTask, paymentMethodsTask);

        Cart = await cartTask;
        if (!Cart.Items.Any())
        {
            TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
            return RedirectToPage("/Cart/Index");
        }

        var paymentMethods = await paymentMethodsTask;
        PaymentMethods = new SelectList(paymentMethods, "Code", "Code");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        try
        {
            var currentCart = await _cartApiService.GetCartAsync();
            if (!currentCart.Items.Any())
            {
                ModelState.AddModelError(string.Empty, "Giỏ hàng của bạn trống, không thể đặt hàng.");
                await OnGetAsync();
                return Page();
            }

            OrderInput.ProductIds = currentCart.Items.Select(i => i.ProductId).ToList();

            var createdOrder = await _orderApiService.CreateOrderAsync(OrderInput);

            return RedirectToPage("/Checkout/Success", new { orderId = createdOrder.Id });
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message ?? "Đã có lỗi xảy ra khi đặt hàng.");
            await OnGetAsync();
            return Page();
        }
    }
}