using Final.UserAPI.DTOs;
using System.Threading.Tasks;

namespace Final.UserAPI.Services
{
    public interface IShoppingCartService
    {
        Task<CartDTO> GetCartByUserIdAsync(long userId);
        Task<CartDTO> AddItemToUserCartAsync(long userId, AddCartItemDTO itemDto);
        Task<CartDTO> RemoveItemFromUserCartAsync(long userId, long productId);
        Task ClearUserCartAsync(long userId);
        Task<CartDTO> UpdateItemQuantityInUserCartAsync(long userId, long productId, int newQuantity);
    }
}