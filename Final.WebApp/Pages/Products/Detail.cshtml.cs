using Final.WebApp.DTOs.Products;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.WebApp.Pages.Products
{

    public class DetailModel : PageModel
    {
        private readonly IProductApiService _productApiService;
        private readonly ILogger<DetailModel> _logger;

        public DetailModel(IProductApiService productApiService, ILogger<DetailModel> logger)
        {
            _productApiService = productApiService;
            _logger = logger;
        }

        public ProductDetailDTO? Product { get; set; }
        public string? ErrorMessage { get; set; }

        // [BindProperty] s? ???c dùng cho nút "Thêm vào gi? hàng" sau này
        // public int Quantity { get; set; } = 1;

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
    }
}