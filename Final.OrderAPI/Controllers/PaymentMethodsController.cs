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
    [ProducesResponseType(typeof(List<PaymentMethodDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllActiveMethodsAsync() 
    {
        var methods = await _service.GetAllActiveMethodsAsync();
        return Ok(methods);
    }
}