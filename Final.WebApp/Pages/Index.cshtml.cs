using Final.WebApp.DTOs.Products;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final.WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly IProductApiService _productApiService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IProductApiService productApiService, ILogger<IndexModel> logger)
    {
        _productApiService = productApiService;
        _logger = logger;
    }

    public PagedResult<ProductDTO> Products { get; set; } = new();
    public SelectList Categories { get; set; } = new(new List<CategoryDTO>(), "Id", "Name");
    public SelectList Brands { get; set; } = new(new List<BrandDTO>(), "Id", "Name"); 
    public string? ErrorMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public ProductQuery Query { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SortOrder { get; set; }

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrEmpty(SortOrder))
        {
            var parts = SortOrder.Split('_');
            if (parts.Length == 2)
            {
                Query.SortBy = parts[0];
                Query.SortDirection = parts[1];
            }
        }

        try
        {
            var productsTask = _productApiService.GetProductsAsync(Query);
            var categoriesTask = _productApiService.GetCategoriesAsync();
            var brandsTask = _productApiService.GetBrandsAsync(); 

            await Task.WhenAll(productsTask, categoriesTask, brandsTask);

            Products = await productsTask;

            var categoryList = await categoriesTask;
            Categories = new SelectList(categoryList, "Id", "Name", Query.CategoryId);

            var brandList = await brandsTask; 
            Brands = new SelectList(brandList, "Id", "Name", Query.BrandId); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra khi tải trang chủ.");
            ErrorMessage = "Không thể tải trước danh sách sản phẩm. Vui lòng thử lại sau.";
        }
    }
}