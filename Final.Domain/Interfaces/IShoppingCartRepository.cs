using Final.Domain.Entities;
using System.Threading.Tasks;

namespace Final.Domain.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetOrCreateCartByUserIdAsync(long userId);
        Task<ShoppingCart> AddOrUpdateItemAsync(long userId, long productId, int quantity);
        Task<bool> RemoveItemAsync(long userId, long productId);
        Task<bool> ClearCartAsync(long userId);
        Task<int> SaveChangesAsync();
        Task RemoveItemsAsync(long userId, List<long> productIds);
    }
}