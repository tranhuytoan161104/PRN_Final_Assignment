using Final.WebApp.DTOs.Products;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final.WebApp.Pages.Admin.Products;

[Authorize(Roles = "Admin, Owner")]
public class EditModel : PageModel
{
    private readonly IProductApiService _productApiService;
    public EditModel(IProductApiService productApiService)
    {
        _productApiService = productApiService;
    }

    [BindProperty]
    public ProductUpdateDTO Product { get; set; } = new();

    public ProductDetailDTO ProductDetail { get; set; } = new();

    public SelectList Categories { get; set; } = default!;
    public SelectList Brands { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(long id)
    {
        try
        {
            var productDetailTask = _productApiService.GetProductDetailAsync(id);
            var categoriesTask = _productApiService.GetCategoriesAsync();
            var brandsTask = _productApiService.GetBrandsAsync();

            await Task.WhenAll(productDetailTask, categoriesTask, brandsTask);

            var productDetail = await productDetailTask;
            ProductDetail = productDetail;

            Product = new ProductUpdateDTO
            {
                Name = productDetail.Name,
                Description = productDetail.Description,
                Price = productDetail.Price,
                CategoryId = productDetail.CategoryId,
                BrandId = productDetail.BrandId
            };

            var categories = await categoriesTask;
            var brands = await brandsTask;

            Categories = new SelectList(categories, "Id", "Name", productDetail.CategoryId);
            Brands = new SelectList(brands, "Id", "Name", productDetail.BrandId);

            return Page();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync(long id)
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(id);
            return Page();
        }
        try
        {
            await _productApiService.UpdateProductAsync(id, Product);
            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
            return RedirectToPage("/Admin/Products/Index");
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await OnGetAsync(id);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostArchiveAsync(long id)
    {
        try
        {
            await _productApiService.ArchiveProductAsync(id);
            TempData["SuccessMessage"] = "Đã lưu trữ sản phẩm.";
        }
        catch (HttpRequestException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToPage(new { id });
    }
}