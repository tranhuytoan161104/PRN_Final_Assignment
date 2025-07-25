using Final.Domain.Entities;
using System.Threading.Tasks;

namespace Final.Domain.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetOrCreateCartForUserAsync(long userId);
        Task<ShoppingCart> AddOrUpdateItemToUserCartAsync(long userId, long productId, int quantity);
        Task<bool> RemoveItemFromUserCartAsync(long userId, long productId);
        Task<bool> ClearUserCartAsync(long userId);
        Task RemoveItemsFromUserCartAsync(long userId, List<long> productIds);
        Task<int> UpdateUserCartAsync();
    }
}
