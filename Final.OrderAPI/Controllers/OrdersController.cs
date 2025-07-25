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


        /// <summary>
        /// Phương thức tạo đơn hàng từ giỏ hàng của người dùng.
        /// Cho phép người dùng thanh toán các sản phẩm trong giỏ hàng của họ và tạo một đơn hàng mới.
        /// </summary>
        /// <param name="createOrderDto">Thông tin đơn hàng cần tạo.</param> 
        /// <returns>Trả về đơn hàng đã được tạo.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDTO>> CreateOrderFromCurrentUserCartAsync([FromBody] CreateOrderDTO createOrderDto)
        {
            var order = await _orderService.CreateOrderFromUserCartAsync(CurrentUserId, createOrderDto);
            return CreatedAtRoute("GetUserOrderById", new { id = order.Id }, order);
        }

        /// <summary>
        /// Phương thức lấy danh sách đơn hàng theo ID của người dùng.
        /// Cho phép người dùng xem tất cả các đơn hàng của họ.
        /// </summary>
        /// <param name="query">Thông tin truy vấn để phân trang và lọc đơn hàng.</param>
        /// <returns>Trả về danh sách đơn hàng của người dùng.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<OrderDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<OrderDTO>>> GetCurrentUserOrdersAsync([FromQuery] OrderQuery query)
        {
            var orders = await _orderService.GetOrdersbyUserIdAsync(CurrentUserId, query);
            return Ok(orders);
        }

        /// <summary>
        /// Phương thức lấy chi tiết đơn hàng của người dùng theo ID đơn hàng.
        /// Cho phép người dùng xem thông tin chi tiết một đơn hàng của họ.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}", Name = "GetUserOrderById")]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDTO>> GetCurrentUserOrderDetailAsync(long id)
        {
            var orderDetail = await _orderService.GetOrderDetailByOrderIdAsync(id, CurrentUserId);
            return Ok(orderDetail);
        }

        /// <summary>
        /// Phương thức hủy đơn hàng của người dùng theo ID đơn hàng.
        /// Cho phép người dùng hủy một đơn hàng đã đặt.
        /// </summary>
        /// <param name="id">ID của đơn hàng cần hủy.</param>
        /// <returns>Trả về đơn hàng đã hủy.</returns>
        [HttpPatch("{id:long}/cancel")]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDTO>> CancelCurrentUserOrderAsync(long id)
        {
            var cancelledOrder = await _orderService.CancelUserOrderByOrderIdAsync(id, CurrentUserId);
            return Ok(cancelledOrder);
        }

        /// <summary>
        /// [Admin] Phương thức lấy tất cả đơn hàng.
        /// Cho phép quản trị viên xem tất cả các đơn hàng trong hệ thống.
        /// </summary>
        /// <param name="query">Thông tin truy vấn để phân trang và lọc đơn hàng.</param>
        /// <returns>Trả về danh sách tất cả đơn hàng.</returns>
        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResult<OrderDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<OrderDTO>>> GetAllOrdersAsync([FromQuery] OrderQuery query)
        {
            var orders = await _orderService.GetAllOrdersAsync(query);
            return Ok(orders);
        }

        /// <summary>
        /// Phương thức lấy chi tiết đơn hàng cho quản trị viên theo ID đơn hàng.
        /// Cho phép quản trị viên xem thông tin chi tiết một đơn hàng bất kì cụ thể.
        /// </summary>
        /// <param name="id">ID của đơn hàng cần xem chi tiết.</param>
        /// <returns>Trả về thông tin chi tiết của đơn hàng.</returns>
        [HttpGet("admin/{id:long}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDTO>> GetOrderDetailAsync(long id)
        {
            var order = await _orderService.GetOrderDetailForAdminAsync(id);
            return Ok(order);
        }

        /// <summary>
        /// Phương thức hủy đơn hàng cho quản trị viên theo ID đơn hàng.
        /// Cho phép quản trị viên hủy một đơn hàng đã đặt.
        /// </summary>
        /// <param name="id">ID của đơn hàng cần hủy.</param>
        /// <returns>Trả về đơn hàng đã hủy.</returns>
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

        /// <summary>
        /// Phương thức cập nhật trạng thái đơn hàng cho quản trị viên.
        /// Cho phép quản trị viên cập nhật trạng thái của một đơn hàng cụ thể.
        /// </summary>
        /// <param name="id">ID của đơn hàng cần cập nhật trạng thái.</param>
        /// <param name="statusDto">Thông tin trạng thái mới cần cập nhật.</param>
        /// <returns>Trả về đơn hàng đã được cập nhật trạng thái.</returns>
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