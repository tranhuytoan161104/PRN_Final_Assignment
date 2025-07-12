using Final.Domain.Interfaces;
using Final.Persistence.Repositories;
using Final.UserAPI.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace Final.UserAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository; // <-- Inject repo sản phẩm

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository; // <-- Khởi tạo
        }

        public async Task<CartDTO> GetCartForUserAsync(long userId)
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

        public async Task<CartDTO> AddItemToCartAsync(long userId, AddCartItemDTO itemDto)
        {
            // 1. Kiểm tra sản phẩm có tồn tại không
            var product = await _productRepository.GetByIdWithImagesAsync(itemDto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }

            // 2. KIỂM TRA TỒN KHO
            if (product.StockQuantity < itemDto.Quantity)
            {
                throw new InvalidOperationException($"Không đủ số lượng tồn kho. Chỉ còn {product.StockQuantity} sản phẩm.");
            }

            // 3. Gọi repository để thêm/cập nhật giỏ hàng
            await _shoppingCartRepository.AddOrUpdateItemAsync(userId, itemDto.ProductId, itemDto.Quantity);

            // 4. Lấy lại giỏ hàng đầy đủ để trả về
            return await GetCartForUserAsync(userId);
        }

        public async Task<CartDTO> RemoveItemFromCartAsync(long userId, long productId)
        {
            var success = await _shoppingCartRepository.RemoveItemAsync(userId, productId);

            if (!success)
            {
                // Ném lỗi để Controller biết sản phẩm không tồn tại trong giỏ
                throw new KeyNotFoundException("Sản phẩm không tìm thấy trong giỏ hàng.");
            }

            // Lấy lại giỏ hàng đầy đủ để trả về trạng thái mới nhất
            return await GetCartForUserAsync(userId);
        }

        public async Task ClearCartAsync(long userId)
        {
            await _shoppingCartRepository.ClearCartAsync(userId);
            // Không cần trả về gì vì giỏ hàng đã trống
        }

        // Thêm phương thức này vào class ShoppingCartService
        public async Task<CartDTO> UpdateItemQuantityAsync(long userId, long productId, int newQuantity)
        {
            // 1. Kiểm tra sản phẩm có tồn tại không
            var product = await _productRepository.GetByIdWithImagesAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại.");
            }

            // 2. KIỂM TRA TỒN KHO
            if (product.StockQuantity < newQuantity)
            {
                throw new InvalidOperationException($"Không đủ số lượng tồn kho. Chỉ còn {product.StockQuantity} sản phẩm.");
            }

            // 3. Lấy giỏ hàng của user
            var cart = await _shoppingCartRepository.GetOrCreateCartByUserIdAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tìm thấy trong giỏ hàng.");
            }

            // 4. Cập nhật số lượng và lưu thay đổi
            cartItem.Quantity = newQuantity;
            // Do cartItem được theo dõi bởi EF Core, chỉ cần SaveChanges là đủ
            await _shoppingCartRepository.SaveChanges(); // <-- Bạn cần thêm phương thức này vào repo

            // 5. Lấy lại giỏ hàng đầy đủ để trả về
            return await GetCartForUserAsync(userId);
        }
    }
}