using Final.WebApp.DTOs.Products;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final.WebApp.Pages.Admin.Products;

[Authorize(Roles = "Admin, Owner")]
public class CreateModel : PageModel
{
    private readonly IProductApiService _productApiService;
    public CreateModel(IProductApiService productApiService)
    {
        _productApiService = productApiService;
    }

    [BindProperty]
    public ProductCreationDTO Product { get; set; } = new();
    public SelectList Categories { get; set; } = default!;
    public SelectList Brands { get; set; } = default!;

    public async Task OnGetAsync()
    {
        var categories = await _productApiService.GetCategoriesAsync();
        var brands = await _productApiService.GetBrandsAsync();
        Categories = new SelectList(categories, "Id", "Name");
        Brands = new SelectList(brands, "Id", "Name");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }
        try
        {
            await _productApiService.CreateProductAsync(Product);
            TempData["SuccessMessage"] = "Tạo sản phẩm thành công!";
            return RedirectToPage("/Admin/Products/Index");
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await OnGetAsync();
            return Page();
        }
    }
}