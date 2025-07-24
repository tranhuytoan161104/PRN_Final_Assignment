using Final.OrderAPI.DTOs;
using Final.OrderAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/payment-methods")]
[Produces("application/json")]
public class PaymentMethodsController : ControllerBase
{
    private readonly IPaymentMethodService _service;
    public PaymentMethodsController(IPaymentMethodService service) { _service = service; }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<PaymentMethodDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveMethods() 
    {
        var methods = await _service.GetActiveMethodsAsync();
        return Ok(methods);
    }
}