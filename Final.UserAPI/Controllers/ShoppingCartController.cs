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

        [HttpGet]
        [ProducesResponseType(typeof(CartDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartDTO>> GetCurrentUserCartAsync()
        {
            var cart = await _shoppingCartService.GetCartByUserIdAsync(CurrentUserId);
            return Ok(cart);
        }

        [HttpPost("items")]
        [ProducesResponseType(typeof(CartDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartDTO>> AddItemToCurrentUserCartAsync([FromBody] AddCartItemDTO itemDto)
        {
            var updatedCart = await _shoppingCartService.AddItemToUserCartAsync(CurrentUserId, itemDto);
            return Ok(updatedCart);
        }

        [HttpPut("items/{productId:long}")]
        [ProducesResponseType(typeof(CartDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartDTO>> UpdateItemQuantityInCurrentUserCartAsync(long productId, [FromBody] UpdateCartItemDTO updateDto)
        {
            var updatedCart = await _shoppingCartService.UpdateItemQuantityInUserCartAsync(CurrentUserId, productId, updateDto.Quantity);
            return Ok(updatedCart);
        }

        [HttpDelete("items/{productId:long}")]
        [ProducesResponseType(typeof(CartDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartDTO>> RemoveItemFromCurrentUserCartAsync(long productId)
        {
            var updatedCart = await _shoppingCartService.RemoveItemFromUserCartAsync(CurrentUserId, productId);
            return Ok(updatedCart);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ClearCurrentUserCartAsync()
        {
            await _shoppingCartService.ClearUserCartAsync(CurrentUserId);
            return NoContent();
        }
    }
}