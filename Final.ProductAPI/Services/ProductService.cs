using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.Domain.Enums;
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

            var productDetailDTOs = new ProductDetailDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                Status = product.Status,
                CreatedAt = product.CreatedAt,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                BrandName = product.Brand?.Name,
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
            };

            return productDetailDTOs;
        }

        public async Task<ProductDetailDTO?> CreateProductAsync(ProductCreationDTO productCreationDto)
        {
            var product = new Product
            {
                Name = productCreationDto.Name,
                Description = productCreationDto.Description,
                Price = productCreationDto.Price,
                StockQuantity = productCreationDto.StockQuantity,
                BrandId = productCreationDto.BrandId,
                CategoryId = productCreationDto.CategoryId,
                Images = productCreationDto.Images?
                    .Select(url => new ProductImage { ImageUrl = url })
                    .ToList()
            };

            var createdProduct = await _productRepository.CreateProductAsync(product);

            if (createdProduct == null)
            {
                return null;
            }

            return await GetProductDetailAsync(createdProduct.Id);
        }

        public async Task<ProductDetailDTO?> AddProductQuantityAsync(long productId, UpdateStockQuantityDTO updateStockQuantityDto)
        {
            var product = await _productRepository.GetProductDetailAsync(productId);
            if (product == null) return null;
            product.StockQuantity += updateStockQuantityDto.NewStockQuantity;
            var updatedProduct = await _productRepository.UpdateProductQuantityAsync(product);
            if (updatedProduct == null)
            {
                return null;
            }
            return await GetProductDetailAsync(updatedProduct.Id);

        }

        public async Task<ProductDetailDTO?> ReduceProductQuantityAsync(long productId, UpdateStockQuantityDTO updateStockQuantityDto)
        {
            var product = await _productRepository.GetProductDetailAsync(productId);
            if (product == null) return null;
            product.StockQuantity -= updateStockQuantityDto.NewStockQuantity;
            var updatedProduct = await _productRepository.UpdateProductQuantityAsync(product);
            if (updatedProduct == null)
            {
                return null;
            }
            return await GetProductDetailAsync(updatedProduct.Id);
        }

        public async Task<ProductDetailDTO?> ArchiveProductAsync(long productId)
        {
            var product = await _productRepository.GetProductDetailAsync(productId);

            if (product == null || product.Status == EProductStatus.Archived)
            {
                return null;
            }

            product.Status = EProductStatus.Archived;

            await _productRepository.UpdateProductAsync(product);

            var productDetailDto = new ProductDetailDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                BrandName = product.Brand?.Name,
                CategoryName = product.Category?.Name,
                Images = product.Images?
                    .Select(i => new ProductImageDTO { Id = i.Id, ImageUrl = i.ImageUrl })
                    .ToList(),
                Reviews = product.Reviews?
                    .Select(r => new ReviewDTO
                    {
                        Rating = r.Rating,
                        Comment = r.Comment,
                        CreatedAt = r.CreatedAt,
                        UserName = r.User?.FirstName
                    })
                    .ToList()
            };

            return productDetailDto;
        }

        public async Task<ProductDetailDTO?> UpdateProductAsync(long productId, ProductUpdateDTO dto)
        {
            var product = await _productRepository.GetProductDetailAsync(productId);

            if (product == null || product.Status == EProductStatus.Archived)
            {
                return null;
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.BrandId = dto.BrandId;
            product.CategoryId = dto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateProductAsync(product);

            return await GetProductDetailAsync(productId);
        }
    }
}
