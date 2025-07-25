using Final.Domain.Interfaces;
using Final.UserAPI.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace Final.UserAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Lấy thông tin giỏ hàng của một người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <returns>Thông tin chi tiết giỏ hàng.</returns>
        public async Task<CartDTO> GetCartByUserIdAsync(long userId)
        {
            var cartEntity = await _shoppingCartRepository.GetOrCreateCartByUserIdAsync(userId);
            var cartDto = new CartDTO
            {
                Id = cartEntity.Id,
                UserId = cartEntity.UserId,
                Items = cartEntity.Items?.Select(item => new CartItemDTO
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    ImageUrl = item.Product.Images?.FirstOrDefault()?.ImageUrl
                }).ToList() ?? new List<CartItemDTO>()
            };
            cartDto.TotalPrice = cartDto.Items.Sum(item => item.Price * item.Quantity);
            return cartDto;
        }

        /// <summary>
        /// Thêm một sản phẩm vào giỏ hàng hoặc cập nhật số lượng nếu đã tồn tại.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <param name="itemDto">Thông tin sản phẩm và số lượng cần thêm.</param>
        /// <returns>Thông tin giỏ hàng sau khi đã cập nhật.</returns>
        /// <exception cref="KeyNotFoundException">Ném ngoại lệ nếu sản phẩm không tồn tại.</exception>
        /// <exception cref="InvalidOperationException">Ném ngoại lệ nếu số lượng tồn kho không đủ.</exception>
        public async Task<CartDTO> AddItemToUserCartAsync(long userId, AddCartItemDTO itemDto)
        {
            var product = await _productRepository.GetByIdWithImagesAsync(itemDto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }

            var cart = await _shoppingCartRepository.GetOrCreateCartByUserIdAsync(userId);
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == itemDto.ProductId);
            var quantityInCart = existingItem?.Quantity ?? 0;

            if (product.StockQuantity < (itemDto.Quantity + quantityInCart))
            {
                throw new InvalidOperationException($"Không đủ số lượng tồn kho. Chỉ còn {product.StockQuantity} sản phẩm.");
            }

            await _shoppingCartRepository.AddOrUpdateItemAsync(userId, itemDto.ProductId, itemDto.Quantity);
            return await GetCartByUserIdAsync(userId);
        }

        /// <summary>
        /// Xóa một sản phẩm khỏi giỏ hàng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <param name="productId">ID của sản phẩm cần xóa.</param>
        /// <returns>Thông tin giỏ hàng sau khi đã cập nhật.</returns>
        /// <exception cref="KeyNotFoundException">Ném ngoại lệ nếu sản phẩm không có trong giỏ hàng.</exception>
        public async Task<CartDTO> RemoveItemFromUserCartAsync(long userId, long productId)
        {
            var success = await _shoppingCartRepository.RemoveItemAsync(userId, productId);
            if (!success)
            {
                throw new KeyNotFoundException("Sản phẩm không tìm thấy trong giỏ hàng.");
            }
            return await GetCartByUserIdAsync(userId);
        }

        /// <summary>
        /// Xóa toàn bộ sản phẩm khỏi giỏ hàng của người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        public async Task ClearUserCartAsync(long userId)
        {
            await _shoppingCartRepository.ClearCartAsync(userId);
        }

        /// <summary>
        /// Cập nhật số lượng của một sản phẩm trong giỏ hàng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <param name="productId">ID của sản phẩm cần cập nhật.</param>
        /// <param name="newQuantity">Số lượng mới.</param>
        /// <returns>Thông tin giỏ hàng sau khi đã cập nhật.</returns>
        /// <exception cref="KeyNotFoundException">Ném ngoại lệ nếu sản phẩm không tồn tại hoặc không có trong giỏ hàng.</exception>
        /// <exception cref="InvalidOperationException">Ném ngoại lệ nếu số lượng tồn kho không đủ.</exception>
        public async Task<CartDTO> UpdateItemQuantityInUserCartAsync(long userId, long productId, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                return await RemoveItemFromUserCartAsync(userId, productId);
            }

            var product = await _productRepository.GetByIdWithImagesAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }
            if (product.StockQuantity < newQuantity)
            {
                throw new InvalidOperationException($"Không đủ số lượng tồn kho. Chỉ còn {product.StockQuantity} sản phẩm.");
            }

            var cart = await _shoppingCartRepository.GetOrCreateCartByUserIdAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tìm thấy trong giỏ hàng.");
            }

            cartItem.Quantity = newQuantity;
            await _shoppingCartRepository.SaveChangesAsync();

            return await GetCartByUserIdAsync(userId);
        }
    }
}
