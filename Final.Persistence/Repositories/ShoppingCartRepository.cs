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

        /// <summary>
        /// Lấy hoặc tạo giỏ hàng theo ID người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <returns>Giỏ hàng của người dùng.</returns>
        public async Task<ShoppingCart> GetOrCreateCartByUserIdAsync(long userId)
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

        /// <summary>
        /// Thêm hoặc cập nhật một sản phẩm trong giỏ hàng của người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <param name="productId">ID của sản phẩm.</param>
        /// <param name="quantity">Số lượng sản phẩm cần thêm hoặc cập nhật.</param>
        /// <returns>Giỏ hàng đã được cập nhật.</returns>
        public async Task<ShoppingCart> AddOrUpdateItemAsync(long userId, long productId, int quantity)
        {
            var cart = await GetOrCreateCartByUserIdAsync(userId);
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

        /// <summary>
        /// Xóa một sản phẩm khỏi giỏ hàng của người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <param name="productId">ID của sản phẩm cần xóa.</param>
        /// <returns>Trả về true nếu xóa thành công, false nếu không tìm thấy giỏ hàng hoặc sản phẩm.</returns>
        public async Task<bool> RemoveItemAsync(long userId, long productId)
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

        /// <summary>
        /// Xóa toàn bộ sản phẩm trong giỏ hàng của người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <returns>Trả về true nếu giỏ hàng có sản phẩm và đã xóa thành công, false nếu không có sản phẩm để xóa.</returns>
        public async Task<bool> ClearCartAsync(long userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(sc => sc.Items)
                .FirstOrDefaultAsync(sc => sc.UserId == userId);

            if (cart == null || !cart.Items.Any()) return false;

            _context.ShoppingCartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Xóa nhiều sản phẩm khỏi giỏ hàng của người dùng theo danh sách ID sản phẩm. 
        /// </summary>
        /// <param name="userId">ID của người dùng.</param> 
        /// <param name="productIds">Danh sách ID sản phẩm cần xóa.</param>
        /// <returns>Trả về một tác vụ không đồng bộ.</returns>
        public async Task RemoveItemsAsync(long userId, List<long> productIds)
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

        /// <summary>
        /// Lưu các thay đổi vào cơ sở dữ liệu.
        /// </summary>
        /// <returns>Số lượng bản ghi đã được lưu.</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
