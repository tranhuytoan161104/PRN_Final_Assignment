using Final.Domain.Entities;
using Final.ProductAPI.DTOs;

namespace Final.ProductAPI.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
    }
}
