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

        public async Task<CartDTO> GetCartByUserIdAsync(long userId)
        {
            var cartEntity = await _shoppingCartRepository.GetOrCreateCartForUserAsync(userId);
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

        public async Task<CartDTO> AddItemToUserCartAsync(long userId, AddCartItemDTO itemDto)
        {
            var product = await _productRepository.GetProductByProductIdWithImagesAsync(itemDto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }

            var cart = await _shoppingCartRepository.GetOrCreateCartForUserAsync(userId);
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == itemDto.ProductId);
            var quantityInCart = existingItem?.Quantity ?? 0;

            if (product.StockQuantity < (itemDto.Quantity + quantityInCart))
            {
                throw new InvalidOperationException($"Không đủ số lượng tồn kho. Chỉ còn {product.StockQuantity} sản phẩm.");
            }

            await _shoppingCartRepository.AddOrUpdateItemToUserCartAsync(userId, itemDto.ProductId, itemDto.Quantity);
            return await GetCartByUserIdAsync(userId);
        }

        public async Task<CartDTO> RemoveItemFromUserCartAsync(long userId, long productId)
        {
            var success = await _shoppingCartRepository.RemoveItemFromUserCartAsync(userId, productId);
            if (!success)
            {
                throw new KeyNotFoundException("Sản phẩm không tìm thấy trong giỏ hàng.");
            }
            return await GetCartByUserIdAsync(userId);
        }

        public async Task ClearUserCartAsync(long userId)
        {
            await _shoppingCartRepository.ClearUserCartAsync(userId);
        }

        public async Task<CartDTO> UpdateItemQuantityInUserCartAsync(long userId, long productId, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                return await RemoveItemFromUserCartAsync(userId, productId);
            }

            var product = await _productRepository.GetProductByProductIdWithImagesAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }
            if (product.StockQuantity < newQuantity)
            {
                throw new InvalidOperationException($"Không đủ số lượng tồn kho. Chỉ còn {product.StockQuantity} sản phẩm.");
            }

            var cart = await _shoppingCartRepository.GetOrCreateCartForUserAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tìm thấy trong giỏ hàng.");
            }

            cartItem.Quantity = newQuantity;
            await _shoppingCartRepository.UpdateUserCartAsync();

            return await GetCartByUserIdAsync(userId);
        }
    }
}