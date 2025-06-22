using Final.Domain.Common;
using Final.Domain.Entities;
using Final.ProductAPI.DTOs;

namespace Final.ProductAPI.Services
{
    public interface IProductService
    {
        Task<PagedResult<ProductDTO>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<PagedResult<ProductDTO>> GetProductsByCategoryAsync(int pageNumber, int pageSize, long categoryId);
    }
}
