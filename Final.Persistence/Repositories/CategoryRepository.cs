using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Final.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy tất cả các danh mục từ cơ sở dữ liệu.
        /// </summary> 
        /// <returns>Danh sách các danh mục.</returns>
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
