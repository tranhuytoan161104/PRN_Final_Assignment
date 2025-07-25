using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Final.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Phuong thức này dùng để lấy danh sách sản phẩm với phân trang và lọc theo các tiêu chí.
        /// </summary>
        /// <param name="queries"> Truyền các thông tin phân trang và lọc sản phẩm </param>
        /// <returns name="PagedResult<Product>"> Trả về danh sách sản phẩm đã được phân trang và lọc </returns>
        public async Task<PagedResult<Product>> GetAllProductsAsync(ProductQuery queries)
        {
            var products = _context.Products.AsQueryable();

            if (queries.CategoryId.HasValue)
                products = products.Where(p => p.CategoryId == queries.CategoryId.Value);
            if (queries.BrandId.HasValue)
                products = products.Where(p => p.BrandId == queries.BrandId.Value);
            if (!string.IsNullOrEmpty(queries.Name))
                products = products.Where(p => p.Name.Contains(queries.Name));
            if (queries.MinPrice.HasValue)
                products = products.Where(p => p.Price >= queries.MinPrice.Value);
            if (queries.MaxPrice.HasValue)
                products = products.Where(p => p.Price <= queries.MaxPrice.Value);

            if (!string.IsNullOrEmpty(queries.SortBy))
            {
                bool desc = string.Equals(queries.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (queries.SortBy.ToLower())
                {
                    case "name":
                        products = desc ? products.OrderByDescending(p => p.Name) : products.OrderBy(p => p.Name);
                        break;
                    case "price":
                        products = desc ? products.OrderByDescending(p => p.Price) : products.OrderBy(p => p.Price);
                        break;
                    case "createdat":
                        products = desc ? products.OrderByDescending(p => p.CreatedAt) : products.OrderBy(p => p.CreatedAt);
                        break;
                    default:
                        products = products.OrderBy(p => p.Id);
                        break;
                }
            }
            else
            {
                products = products.OrderBy(p => p.Id);
            }

            var totalItems = await products.CountAsync();
            var items = await products
                .Skip((queries.PageNumber - 1) * queries.PageSize)
                .Take(queries.PageSize)
                .ToListAsync();

            return new PagedResult<Product>
            {
                Items = items,
                PageNumber = queries.PageNumber,
                PageSize = queries.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)queries.PageSize)
            };
        }


        /// <summary>
        /// Phuong thức này dùng để lấy thông tin chi tiết của một sản phẩm dựa trên Id.
        /// </summary>
        /// <param name="productId"> Xác định sản phẩm cần lấy thông tin bằng Id </param>
        /// <returns name="Product"> Trả về thông tin chi tiết của sản phẩm nếu tìm thấy, hoặc null nếu không tìm thấy </returns>
        public async Task<Product?> GetProductByProductIdAsync(long productId)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.Id == productId);
        }


        /// <summary>
        /// Phuong thức này dùng để tạo một sản phẩm mới.
        /// </summary>
        /// <param name="product"> Xác định sản phẩm cần tạo </param>
        /// <returns name="Product"> Trả về sản phẩm đã được tạo </returns>
        public async Task<Product?> CreateProductAsync(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.AddAt = DateTime.UtcNow;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }


        /// <summary>
        /// Phương thức này dùng để cập nhật thông tin bất kì của một sản phẩm.
        /// </summary>
        /// <param name="product"> Xác định sản phẩm cần cập nhật </param>
        /// <returns></returns>
        public async Task UpdateProductAsync(Product product)
        {
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Phương thức này dùng để lấy sản phẩm theo Id và bao gồm các hình ảnh của nó.
        /// </summary>
        /// <param name="productId"> Xác định sản phẩm cần lấy thông tin bằng Id </param>
        /// <returns name="Product"> Trả về sản phẩm bao gồm các hình ảnh của nó nếu tìm thấy, hoặc null nếu không tìm thấy </returns>
        public async Task<Product?> GetProductByProductIdWithImagesAsync(long productId)
        {
            return await _context.Products
                .Include(p => p.Images) 
                .FirstOrDefaultAsync(p => p.Id == productId);
        }
    }
}
