using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.ProductAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Final.ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PagedResult<ProductDTO>> GetAllProductsAsync(ProductQuery query)
        {
            var pagedResultEntity = await _productRepository.GetAllProductsAsync(query);

            var productDTOs = pagedResultEntity.Items?.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
            }).ToList() ?? new List<ProductDTO>();

            return new PagedResult<ProductDTO>
            {
                Items = productDTOs,
                PageNumber = pagedResultEntity.PageNumber,
                PageSize = pagedResultEntity.PageSize,
                TotalItems = pagedResultEntity.TotalItems,
                TotalPages = pagedResultEntity.TotalPages
            };
        }

        public async Task<ProductDetailDTO?> GetProductDetailAsync(long productId)
        {
            var product = await _productRepository.GetProductDetailAsync(productId);
            if (product == null) return null;

            var productDto = new ProductDetailDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                CategoryName = product.Category?.Name,
                Reviews = product.Reviews?.Select(r => new ReviewDTO
                {
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    UserName = r.User?.FirstName 
                }).ToList(),
                Images = product.Images?.Select(i => new ProductImageDTO
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                }).ToList()
                // ...
                // Thêm các thuộc tính khác nếu cần
            };

            return productDto;
        }
    }
}
