using Final.WebApp.DTOs.Products;
using Final.WebApp.DTOs.Carts;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Products
{

    public class DetailModel : PageModel
    {
        private readonly IProductApiService _productApiService;
        private readonly ICartApiService _cartApiService;
        private readonly ILogger<DetailModel> _logger;

        public DetailModel(IProductApiService productApiService, ICartApiService cartApiService, ILogger<DetailModel> logger)
        {
            _productApiService = productApiService;
            _cartApiService = cartApiService;
            _logger = logger;
        }

        public ProductDetailDTO? Product { get; set; }
        public string? ErrorMessage { get; set; }

        public int Quantity { get; set; } = 1;

        public async Task<IActionResult> OnGetAsync(long id)
        {
            try
            {
                Product = await _productApiService.GetProductDetailAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Không tìm thấy sản phẩm với Id: {ProductId}", id);
                ErrorMessage = ex.Message; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi tải chi tiết sản phẩm với Id: {ProductId}", id);
                ErrorMessage = "Đã có lỗi không mong muốn khi tải chi tiết sản phẩm. Xin vui lòng thử lại.";
            }

            return Page();
        }

        [Authorize] 
        public async Task<IActionResult> OnPostAsync(long id)
        {
            var product = await _productApiService.GetProductDetailAsync(id);
            if (Quantity > product.StockQuantity)
            {
                TempData["ErrorMessage"] = $"Số lượng tồn kho không đủ. Chỉ còn {product.StockQuantity} sản phẩm.";
                return RedirectToPage(new { id });
            }

            try
            {
                var item = new AddCartItemDTO { ProductId = id, Quantity = this.Quantity };
                await _cartApiService.AddItemToCartAsync(item);

                TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng thành công!";
                return RedirectToPage("/Cart/Index");
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage(new { id });
            }
        }
    }
}