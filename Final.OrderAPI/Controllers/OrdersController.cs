using Final.Domain.Enums;
using Final.OrderAPI.DTOs;
using Final.OrderAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Final.OrderAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private long CurrentUserId => long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public OrdersController(IOrderService orderService) { _orderService = orderService; }

        [HttpPost]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDto>> CreateOrderAsync([FromBody] CreateOrderDto createOrderDto)
        {
            var order = await _orderService.CreateOrderFromCartAsync(CurrentUserId, createOrderDto);
            return CreatedAtRoute("GetUserOrderById", new { id = order.Id }, order);
        }

        [HttpGet("{id:long}", Name = "GetUserOrderById")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrderById(long id) 
        {
            var orderDetail = await _orderService.GetUserOrderDetailAsync(id, CurrentUserId);
            return Ok(orderDetail);
        }

        [HttpGet("admin/{id:long}", Name = "GetOrderForAdminById")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrderDetail(long id) 
        {
            var order = await _orderService.GetOrderDetailForAdminAsync(id);
            return Ok(order);
        }

        [HttpPatch("{id}/cancel")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> CancelOrder(long id) 
        {
            var cancelledOrder = await _orderService.CancelUserOrderAsync(id, CurrentUserId);
            return Ok(cancelledOrder);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> UpdateOrderStatus(long id, [FromBody] UpdateOrderStatusDto statusDto) 
        {
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, statusDto.Status);
            return Ok(updatedOrder);
        }
    }
}