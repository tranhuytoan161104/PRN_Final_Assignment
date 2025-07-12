using Final.UserAPI.DTOs;
using System.Threading.Tasks;

namespace Final.UserAPI.Services
{
    public interface IShoppingCartService
    {
        Task<CartDTO> GetCartForUserAsync(long userId);
        Task<CartDTO> AddItemToCartAsync(long userId, AddCartItemDTO itemDto);
        Task<CartDTO> RemoveItemFromCartAsync(long userId, long productId);
        Task ClearCartAsync(long userId);
        Task<CartDTO> UpdateItemQuantityAsync(long userId, long productId, int newQuantity);
    }
}