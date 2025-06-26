using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Product>> GetAllProductsAsync(ProductQuery query)
        {
            var products = _context.Products.AsQueryable();

            // Điều kiện lọc
            if (query.CategoryId.HasValue)
                products = products.Where(p => p.CategoryId == query.CategoryId.Value);
            if (query.BrandId.HasValue)
                products = products.Where(p => p.BrandId == query.BrandId.Value);
            if (!string.IsNullOrEmpty(query.Name))
                products = products.Where(p => p.Name.Contains(query.Name));
            if (query.MinPrice.HasValue)
                products = products.Where(p => p.Price >= query.MinPrice.Value);
            if (query.MaxPrice.HasValue)
                products = products.Where(p => p.Price <= query.MaxPrice.Value);

            // ...
            // Thêm các điều kiện lọc khác nếu cần


            // Điều kiện sắp xếp
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                bool desc = string.Equals(query.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                switch (query.SortBy.ToLower())
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

                    // ...
                    // Thêm các trường sắp xếp khác nếu cần
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
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Product>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)
            };
        }

        public async Task<Product?> GetProductDetailAsync(long productId)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }
    }
}
