using Final.Domain.Common;
using Final.Domain.Queries;
using Final.OrderAPI.DTOs;
using Final.OrderAPI.Services;
using Final.PaymentAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final.OrderAPI.Controllers
{
    [ApiController]
    [Route("api/admin/transactions")]
    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    public class PaymentTransactionsController : ControllerBase
    {
        private readonly IPaymentTransactionService _service;
        public PaymentTransactionsController(IPaymentTransactionService service) { _service = service; }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<PaymentTransactionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTransactions([FromQuery] PaymentTransactionQuery query) 
        {
            var transactions = await _service.GetAllAsync(query);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentTransactionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionById(long id) 
        {
            var transaction = await _service.GetByIdAsync(id);
            return Ok(transaction);
        }
    }
}