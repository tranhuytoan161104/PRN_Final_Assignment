using Final.OrderAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/payment-methods")]
public class PaymentMethodsController : ControllerBase
{
    private readonly IPaymentMethodService _service;
    public PaymentMethodsController(IPaymentMethodService service) { _service = service; }

    [HttpGet]
    public async Task<IActionResult> GetActiveMethods()
    {
        var methods = await _service.GetActiveMethodsAsync();
        return Ok(methods);
    }
}