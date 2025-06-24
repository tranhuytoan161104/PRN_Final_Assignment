using Final.Domain.Common;
using Final.ProductAPI.DTOs;
using Final.Domain.Queries;

namespace Final.ProductAPI.Services
{
    public interface IProductService
    {
        Task<PagedResult<ProductDTO>> GetAllProductsAsync(ProductQuery query);
        Task<ProductDetailDTO?> GetProductDetailAsync(long productId);
    }
}
