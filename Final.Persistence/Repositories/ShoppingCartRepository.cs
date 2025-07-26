using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Persistence.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> GetOrCreateCartForUserAsync(long userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(sc => sc.Items)
                    .ThenInclude(item => item.Product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<ShoppingCart> AddOrUpdateItemToUserCartAsync(long userId, long productId, int quantity)
        {
            var cart = await GetOrCreateCartForUserAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new ShoppingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ShoppingCartId = cart.Id
                };
                cart.Items.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> RemoveItemFromUserCartAsync(long userId, long productId)
        {
            var cart = await _context.ShoppingCarts
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);

            if (cart == null) return false;

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null) return false;

            _context.ShoppingCartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearUserCartAsync(long userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);

            if (cart == null || !cart.Items.Any()) return false;

            _context.ShoppingCartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task RemoveItemsFromUserCartAsync(long userId, List<long> productIds)
        {
            var cart = await _context.ShoppingCarts
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);

            if (cart?.Items == null) return;

            var itemsToRemove = cart.Items.Where(item => productIds.Contains(item.ProductId)).ToList();
            if (itemsToRemove.Any())
            {
                _context.ShoppingCartItems.RemoveRange(itemsToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> UpdateUserCartAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}