using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.ProductAPI.DTOs;

namespace Final.ProductAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Lấy tất cả các danh mục sản phẩm.
        /// </summary>
        /// <returns>Một danh sách các danh mục dưới dạng DTO.</returns>
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
        }
    }
}
