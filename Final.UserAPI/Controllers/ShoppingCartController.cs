using Final.UserAPI.DTOs;
using Final.UserAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Final.UserAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    [Produces("application/json")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        private long CurrentUserId => long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        /// <summary>
        /// Lấy thông tin giỏ hàng của người dùng hiện tại.
        /// </summary>
        /// <returns>Thông tin chi tiết của giỏ hàng.</returns>
        /// <response code="200">Trả về giỏ hàng thành công.</response>
        /// <response code="401">Nếu người dùng chưa đăng nhập.</response>
        [HttpGet]
        [ProducesResponseType(typeof(CartDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartDTO>> GetCurrentUserCartAsync()
        {
            var cart = await _shoppingCartService.GetCartByUserIdAsync(CurrentUserId);
            return Ok(cart);
        }

        /// <summary>
        /// Thêm một sản phẩm vào giỏ hàng.
        /// </summary>
        /// <param name="itemDto">Thông tin sản phẩm và số lượng cần thêm.</param>
        /// <returns>Thông tin giỏ hàng sau khi đã cập nhật.</returns>
        /// <response code="200">Thêm sản phẩm thành công.</response>
        /// <response code="400">Nếu dữ liệu không hợp lệ hoặc số lượng tồn kho không đủ.</response>
        /// <response code="404">Nếu sản phẩm không tồn tại.</response>
        [HttpPost("items")]
        [ProducesResponseType(typeof(CartDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartDTO>> AddItemToCurrentUserCartAsync([FromBody] AddCartItemDTO itemDto)
        {
            var updatedCart = await _shoppingCartService.AddItemToUserCartAsync(CurrentUserId, itemDto);
            return Ok(updatedCart);
        }

        /// <summary>
        /// Cập nhật số lượng của một sản phẩm trong giỏ hàng.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần cập nhật.</param>
        /// <param name="updateDto">Số lượng mới.</param>
        /// <returns>Thông tin giỏ hàng sau khi đã cập nhật.</returns>
        /// <response code="200">Cập nhật thành công.</response>
        /// <response code="400">Nếu dữ liệu không hợp lệ hoặc số lượng tồn kho không đủ.</response>
        /// <response code="404">Nếu sản phẩm không tồn tại hoặc không có trong giỏ hàng.</response>
        [HttpPut("items/{productId:long}")]
        [ProducesResponseType(typeof(CartDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartDTO>> UpdateItemQuantityInCurrentUserCartAsync(long productId, [FromBody] UpdateCartItemDTO updateDto)
        {
            var updatedCart = await _shoppingCartService.UpdateItemQuantityInUserCartAsync(CurrentUserId, productId, updateDto.Quantity);
            return Ok(updatedCart);
        }

        /// <summary>
        /// Xóa một sản phẩm khỏi giỏ hàng.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần xóa.</param>
        /// <returns>Thông tin giỏ hàng sau khi đã cập nhật.</returns>
        /// <response code="200">Xóa sản phẩm thành công.</response>
        /// <response code="404">Nếu sản phẩm không có trong giỏ hàng.</response>
        [HttpDelete("items/{productId:long}")]
        [ProducesResponseType(typeof(CartDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartDTO>> RemoveItemFromCurrentUserCartAsync(long productId)
        {
            var updatedCart = await _shoppingCartService.RemoveItemFromUserCartAsync(CurrentUserId, productId);
            return Ok(updatedCart);
        }

        /// <summary>
        /// Xóa tất cả các sản phẩm khỏi giỏ hàng.
        /// </summary>
        /// <returns>Không có nội dung trả về.</returns>
        /// <response code="204">Xóa giỏ hàng thành công.</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ClearCurrentUserCartAsync()
        {
            await _shoppingCartService.ClearUserCartAsync(CurrentUserId);
            return NoContent();
        }
    }
}
