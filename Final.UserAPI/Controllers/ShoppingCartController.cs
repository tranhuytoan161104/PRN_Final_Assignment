using Final.UserAPI.DTOs;
using Final.UserAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Final.UserAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token không hợp lệ.");
            }

            var cart = await _shoppingCartService.GetCartForUserAsync(userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddCartItemDTO itemDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token không hợp lệ.");
            }

            try
            {
                var updatedCart = await _shoppingCartService.AddItemToCartAsync(userId, itemDto);
                return Ok(updatedCart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> RemoveItemFromCart(long productId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token không hợp lệ.");
            }

            try
            {
                var updatedCart = await _shoppingCartService.RemoveItemFromCartAsync(userId, productId);
                return Ok(updatedCart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token không hợp lệ.");
            }

            await _shoppingCartService.ClearCartAsync(userId);

            // Trả về 204 No Content là một best practice cho
            // một hành động DELETE thành công mà không cần trả về nội dung.
            return NoContent();
        }

        // Thêm phương thức này vào class ShoppingCartController
        [HttpPut("items/{productId}")]
        public async Task<IActionResult> UpdateItemQuantity(long productId, [FromBody] UpdateCartItemDTO updateDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Token không hợp lệ.");
            }

            try
            {
                var updatedCart = await _shoppingCartService.UpdateItemQuantityAsync(userId, productId, updateDto.Quantity);
                return Ok(updatedCart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}