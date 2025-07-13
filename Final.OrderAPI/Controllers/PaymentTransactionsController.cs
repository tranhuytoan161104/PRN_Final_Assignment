using Final.Domain.Common;
using Final.Domain.Queries;
using Final.OrderAPI.Services;
using Final.PaymentAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Final.OrderAPI.Controllers
{
    [ApiController]
    [Route("api/admin/transactions")]
    [Authorize(Roles = "Admin")]
    public class PaymentTransactionsController : Controller
    {
        private readonly IPaymentTransactionService _service;

        public PaymentTransactionsController(IPaymentTransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] PaymentTransactionQuery query)
        {
            var transactions = await _service.GetAllAsync(query);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(long id)
        {
            var transaction = await _service.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
    }
}
