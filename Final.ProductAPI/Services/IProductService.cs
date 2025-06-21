using Final.Domain.Common;
using Final.ProductAPI.DTOs;

namespace Final.ProductAPI.Services
{
    public interface IProductService
    {
        Task<PagedResult<ProductDTO>> GetAllProductsAsync(int pageNumber, int pageSize);
    }
}
