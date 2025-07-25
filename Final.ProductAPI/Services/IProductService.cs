using Final.Domain.Common;
using Final.Domain.Queries;
using Final.ProductAPI.DTOs;

namespace Final.ProductAPI.Services
{
    public interface IProductService
    {
        Task<PagedResult<ProductDTO>> GetAllProductsAsync(ProductQuery query);
        Task<ProductDetailDTO?> GetProductDetailByProductIdAsync(long productId);
        Task<ProductDetailDTO?> CreateProductAsync(ProductCreationDTO productCreationDto);
        Task<ProductDetailDTO?> UpdateProductStockQuantityAsync(long productId, StockQuantityUpdateDTO updateStockQuantityDto);
        Task<ProductDetailDTO?> ArchiveProductAsync(long productId);
        Task<ProductDetailDTO?> UpdateProductDetailAsync(long productId, ProductUpdateDTO productUpdateDto);
    }
}
