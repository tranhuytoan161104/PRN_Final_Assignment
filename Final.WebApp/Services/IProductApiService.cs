using Final.WebApp.DTOs.Products;

namespace Final.WebApp.Services
{
    public interface IProductApiService
    {
        Task<PagedResult<ProductDTO>> GetProductsAsync(ProductQuery query);
        Task<List<CategoryDTO>> GetCategoriesAsync();
        Task<List<BrandDTO>> GetBrandsAsync();
        Task<ProductDetailDTO> GetProductDetailAsync(long productId);
    }
}
