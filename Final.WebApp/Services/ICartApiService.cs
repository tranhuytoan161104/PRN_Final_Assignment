using Final.WebApp.DTOs.Carts;

namespace Final.WebApp.Services
{
    public interface ICartApiService
    {
        Task<CartDTO> GetCartAsync();
        Task<CartDTO> AddItemToCartAsync(AddCartItemDTO item);
        Task<CartDTO> UpdateItemQuantityAsync(long productId, int quantity);
        Task<CartDTO> RemoveItemFromCartAsync(long productId);
    }
}
