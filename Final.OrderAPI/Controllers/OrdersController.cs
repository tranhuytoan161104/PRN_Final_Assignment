using Final.Domain.Common;
using Final.Domain.Enums;
using Final.Domain.Queries;
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

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<OrderDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<OrderDto>>> GetUserOrdersAsync([FromQuery] OrderQuery query)
        {
            var orders = await _orderService.GetUserOrdersAsync(CurrentUserId, query);
            return Ok(orders);
        }

        [HttpGet("{id:long}", Name = "GetUserOrderById")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(long id)
        {
            var orderDetail = await _orderService.GetUserOrderDetailAsync(id, CurrentUserId);
            return Ok(orderDetail);
        }

        [HttpPatch("{id:long}/cancel")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> CancelMyOrderAsync(long id)
        {
            var cancelledOrder = await _orderService.CancelOrderForCurrentUserAsync(id, CurrentUserId);
            return Ok(cancelledOrder);
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResult<OrderDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<OrderDto>>> GetAllOrdersForAdminAsync([FromQuery] OrderQuery query)
        {
            var orders = await _orderService.GetAllOrdersAsync(query);
            return Ok(orders);
        }

        [HttpGet("admin/{id:long}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrderDetailForAdminAsync(long id)
        {
            var order = await _orderService.GetOrderDetailForAdminAsync(id);
            return Ok(order);
        }

        [HttpPatch("admin/{id:long}/cancel")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> CancelOrderForAdminAsync(long id)
        {
            var cancelledOrder = await _orderService.CancelOrderForAdminAsync(id);
            return Ok(cancelledOrder);
        }

        [HttpPatch("admin/{id:long}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> UpdateOrderStatusAsync(long id, [FromBody] UpdateOrderStatusDto statusDto)
        {
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, statusDto.Status);
            return Ok(updatedOrder);
        }
    }
}