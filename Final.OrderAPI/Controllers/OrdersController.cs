using Final.OrderAPI.DTOs;
using Final.OrderAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Final.OrderAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService) { _orderService = orderService; }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out var userId))
                return Unauthorized("Token không hợp lệ.");

            try
            {
                var order = await _orderService.CreateOrderFromCartAsync(userId, createOrderDto);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}", Name = "GetOrderById")]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out var userId))
                return Unauthorized("Token không hợp lệ.");

            var orderDetail = await _orderService.GetUserOrderDetailAsync(id, userId);

            if (orderDetail == null)
            {
                // Trả về 404 Not Found là best practice.
                // Không nên trả về 403 Forbidden để tránh lộ thông tin đơn hàng tồn tại.
                return NotFound($"Không tìm thấy đơn hàng với ID: {id}");
            }

            return Ok(orderDetail);
        }

        [HttpPatch("{id}/cancel")]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> CancelOrder(long id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out var userId))
                return Unauthorized("Token không hợp lệ.");

            try
            {
                var cancelledOrder = await _orderService.CancelUserOrderAsync(id, userId);
                return Ok(cancelledOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Bắt lỗi khi đơn hàng không ở trạng thái 'Pending'
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderDetail(long id)
        {
            var order = await _orderService.GetOrderDetailForAdminAsync(id);
            if (order == null)
            {
                return NotFound($"Không tìm thấy đơn hàng với ID: {id}");
            }
            return Ok(order);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(long id, [FromBody] UpdateOrderStatusDto statusDto)
        {
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, statusDto.Status);
            if (updatedOrder == null)
            {
                return NotFound($"Không tìm thấy đơn hàng với ID: {id}");
            }
            return Ok(updatedOrder);
        }
    }
}
