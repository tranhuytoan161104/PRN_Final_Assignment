using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> GetAllProductsAsync(ProductQuery query);
        Task<Product?> GetProductDetailAsync(long productId);
        Task<Product?> CreateProductAsync(Product product); 
        Task<Product?> UpdateProductQuantityAsync(Product product);
        //Task<Product?> UpdateProductStatusAsync(long productId);
        //Task<Product?> UpdateProductDetailAsync(long productId);
    }
}
