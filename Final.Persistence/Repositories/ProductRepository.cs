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

        public async Task<Product?> CreateProductAsync(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.AddAt = DateTime.UtcNow;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetProductByProductIdWithImagesAsync(long productId)
        {
            return await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }
    }
}