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
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDTO>> CreateOrderFromCurrentUserCartAsync([FromBody] CreateOrderDTO createOrderDto)
        {
            var order = await _orderService.CreateOrderFromUserCartAsync(CurrentUserId, createOrderDto);
            return CreatedAtRoute("GetUserOrderById", new { id = order.Id }, order);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<OrderDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<OrderDTO>>> GetCurrentUserOrdersAsync([FromQuery] OrderQuery query)
        {
            var orders = await _orderService.GetOrdersbyUserIdAsync(CurrentUserId, query);
            return Ok(orders);
        }

        [HttpGet("{id:long}", Name = "GetUserOrderById")]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDTO>> GetCurrentUserOrderDetailAsync(long id)
        {
            var orderDetail = await _orderService.GetOrderDetailByOrderIdAsync(id, CurrentUserId);
            return Ok(orderDetail);
        }

        [HttpPatch("{id:long}/cancel")]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDTO>> CancelCurrentUserOrderAsync(long id)
        {
            var cancelledOrder = await _orderService.CancelUserOrderByOrderIdAsync(id, CurrentUserId);
            return Ok(cancelledOrder);
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResult<OrderDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<OrderDTO>>> GetAllOrdersAsync([FromQuery] OrderQuery query)
        {
            var orders = await _orderService.GetAllOrdersAsync(query);
            return Ok(orders);
        }

        [HttpGet("admin/{id:long}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDTO>> GetOrderDetailAsync(long id)
        {
            var order = await _orderService.GetOrderDetailForAdminAsync(id);
            return Ok(order);
        }

        [HttpPatch("admin/{id:long}/cancel")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDTO>> CancelOrderAsync(long id)
        {
            var cancelledOrder = await _orderService.CancelOrderForAdminAsync(id);
            return Ok(cancelledOrder);
        }

        [HttpPatch("admin/{id:long}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDTO>> UpdateOrderStatusAsync(long id, [FromBody] UpdateOrderStatusDTO statusDto)
        {
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, statusDto.Status);
            return Ok(updatedOrder);
        }
    }
}