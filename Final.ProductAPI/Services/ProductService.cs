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

        public async Task<ProductDetailDTO?> GetProductDetailByProductIdAsync(long productId)
        {
            var product = await _productRepository.GetProductByProductIdAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {productId}.");
            }

            return MapToProductDetailDTO(product);
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

            product.Status = product.StockQuantity > 0 ? EProductStatus.Available : EProductStatus.OutOfStock;

            var createdProduct = await _productRepository.CreateProductAsync(product);
            if (createdProduct == null)
            {
                throw new InvalidOperationException("Tạo sản phẩm mới thất bại.");
            }

            var detailedProduct = await _productRepository.GetProductByProductIdAsync(createdProduct.Id);
            return MapToProductDetailDTO(detailedProduct!);
        }

        public async Task<ProductDetailDTO?> UpdateProductDetailAsync(long productId, ProductUpdateDTO productUpdateDto)
        {
            var product = await _productRepository.GetProductByProductIdWithImagesAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {productId} để cập nhật.");
            }

            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;
            product.BrandId = productUpdateDto.BrandId;
            product.CategoryId = productUpdateDto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            var existingImageUrls = product.Images.Select(i => i.ImageUrl).ToList();
            var newImageUrls = productUpdateDto.Images ?? new List<string>();

            var imagesToDelete = product.Images.Where(img => !newImageUrls.Contains(img.ImageUrl)).ToList();
            foreach (var image in imagesToDelete) { product.Images.Remove(image); }

            var urlsToAdd = newImageUrls.Where(url => !existingImageUrls.Contains(url)).ToList();
            foreach (var url in urlsToAdd) { product.Images.Add(new ProductImage { ImageUrl = url }); }

            await _productRepository.UpdateProductAsync(product);

            var detailedProduct = await _productRepository.GetProductByProductIdAsync(productId);
            return MapToProductDetailDTO(detailedProduct!);
        }

        public async Task<ProductDetailDTO?> UpdateProductStockQuantityAsync(long productId, StockQuantityUpdateDTO updateStockQuantityDto)
        {
            var product = await _productRepository.GetProductByProductIdAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {productId}.");
            }
            if (product.Status == EProductStatus.Archived)
            {
                throw new InvalidOperationException("Không thể cập nhật số lượng cho sản phẩm đã được lưu trữ.");
            }

            var newStock = product.StockQuantity + updateStockQuantityDto.ChangeQuantity;
            if (newStock < 0)
            {
                throw new InvalidOperationException($"Số lượng tồn kho không đủ. Chỉ còn {product.StockQuantity} sản phẩm.");
            }

            product.StockQuantity = newStock;
            product.Status = product.StockQuantity > 0 ? EProductStatus.Available : EProductStatus.OutOfStock;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateProductAsync(product);

            return MapToProductDetailDTO(product);
        }

        public async Task<ProductDetailDTO?> ArchiveProductAsync(long productId)
        {
            var product = await _productRepository.GetProductByProductIdAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {productId}.");
            }
            if (product.Status == EProductStatus.Archived)
            {
                throw new InvalidOperationException($"Sản phẩm với ID {productId} đã được lưu trữ từ trước.");
            }

            product.Status = EProductStatus.Archived;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateProductAsync(product);

            return MapToProductDetailDTO(product);
        }

        private ProductDetailDTO MapToProductDetailDTO(Product product)
        {
            return new ProductDetailDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                Status = product.Status,
                CreatedAt = product.CreatedAt,
                AddAt = product.AddAt,
                UpdatedAt = product.UpdatedAt,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                BrandName = product.Brand?.Name,
                CategoryName = product.Category?.Name,
                Reviews = product.Reviews?.Select(r => new ProductReviewDTO
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
        }
    }
}