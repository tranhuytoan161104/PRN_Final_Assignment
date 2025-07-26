using Final.WebApp.DTOs.Products;
using Final.WebApp.Services;
using Final.WebApp.DTOs.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final.WebApp.Pages.Admin.Products;

[Authorize(Roles = "Admin, Owner")]
public class IndexModel : PageModel
{
    private readonly IProductApiService _productApiService;
    public IndexModel(IProductApiService productApiService)
    {
        _productApiService = productApiService;
    }

    // --- Dữ liệu để hiển thị ---
    public PagedResult<ProductDTO> Products { get; set; } = new();
    public SelectList Categories { get; set; } = default!;
    public SelectList Brands { get; set; } = default!;

    // --- Dữ liệu binding từ URL ---
    [BindProperty(SupportsGet = true)]
    public ProductQuery Query { get; set; } = new() { PageSize = 10 }; // Mặc định 10 sản phẩm/trang

    [BindProperty(SupportsGet = true)]
    public string? SortOrder { get; set; }

    public async Task OnGetAsync()
    {
        // Xử lý logic sắp xếp
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
            // Lấy song song dữ liệu để tối ưu
            var productsTask = _productApiService.GetProductsAsync(Query);
            var categoriesTask = _productApiService.GetCategoriesAsync();
            var brandsTask = _productApiService.GetBrandsAsync();

            await Task.WhenAll(productsTask, categoriesTask, brandsTask);

            Products = await productsTask;
            var categoryList = await categoriesTask;
            var brandList = await brandsTask;

            Categories = new SelectList(categoryList, "Id", "Name", Query.CategoryId);
            Brands = new SelectList(brandList, "Id", "Name", Query.BrandId);
        }
        catch (HttpRequestException ex)
        {
            // (Tùy chọn) Ghi log lỗi ở đây
            ModelState.AddModelError(string.Empty, $"Lỗi khi tải dữ liệu: {ex.Message}");
        }
    }
}