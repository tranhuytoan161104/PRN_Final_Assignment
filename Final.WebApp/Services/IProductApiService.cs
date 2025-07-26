using Final.WebApp.DTOs.Common;
using Final.WebApp.DTOs.Products;

namespace Final.WebApp.Services
{
    public interface IProductApiService
    {
        Task<PagedResult<ProductDTO>> GetProductsAsync(ProductQuery query);
        Task<List<CategoryDTO>> GetCategoriesAsync();
        Task<List<BrandDTO>> GetBrandsAsync();
        Task<ProductDetailDTO> GetProductDetailAsync(long productId);
        Task<ProductDetailDTO> CreateProductAsync(ProductCreationDTO newProduct);
        Task<ProductDetailDTO> UpdateProductAsync(long productId, ProductUpdateDTO productToUpdate);
        Task ArchiveProductAsync(long productId);
    }
}
