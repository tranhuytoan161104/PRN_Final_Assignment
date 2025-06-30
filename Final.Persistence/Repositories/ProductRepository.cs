using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.Domain.Enums;
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
            var products = _context.Products
                .Where(p => !p.Status.Equals("Archive"))
                .AsQueryable();

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
                .Where(p => !p.Status.Equals("Archive"))
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

        public async Task<Product?> UpdateProductQuantityAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null) return null;
            existingProduct.StockQuantity = product.StockQuantity;
            if (product.StockQuantity <= 0)
            {
                existingProduct.Status = EProductStatus.OutOfStock;
            }
            else
            {
                existingProduct.Status = EProductStatus.Available;
            }
            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _context.SaveChangesAsync();
        }
    }
}
