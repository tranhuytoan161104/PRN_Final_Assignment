using Final.Domain.Common;
using Final.Domain.Queries;
using Final.ProductAPI.DTOs;

namespace Final.ProductAPI.Services
{
    public interface IProductService
    {
        Task<PagedResult<ProductDTO>> GetAllProductsAsync(ProductQuery query);
        Task<ProductDetailDTO?> GetProductDetailAsync(long productId);
        Task<ProductDetailDTO?> CreateProductAsync(ProductCreationDTO productCreationDto);
        Task<ProductDetailDTO?> AddProductQuantityAsync(long productId, UpdateStockQuantityDTO updateStockQuantityDto);
        Task<ProductDetailDTO?> ReduceProductQuantityAsync(long productId, UpdateStockQuantityDTO updateStockQuantityDto);
        Task<ProductDetailDTO?> ArchiveProductAsync(long productId);
        Task<ProductDetailDTO?> UpdateProductAsync(long productId, ProductUpdateDTO productUpdateDto);
    }
}
