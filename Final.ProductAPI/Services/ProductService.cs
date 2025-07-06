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


        /// <summary>
        /// Phương thức này dùng để lấy danh sách sản phẩm với phân trang và lọc theo các tiêu chí.
        /// </summary>
        /// <param name="query"> Truyền các thông tin phân trang và lọc sản phẩm </param>
        /// <returns name="PagedResult<ProductDTO>"> Trả về danh sách sản phẩm đã được chuyển đổi sang DTO </returns>
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


        /// <summary>
        /// Phương thức này dùng để lấy thông tin chi tiết của một sản phẩm dựa trên Id.
        /// </summary>
        /// <param name="productId"> Xác định sản phẩm cần lấy thông tin bằng Id </param>
        /// <returns name="ProductDetailDTO"> Trả về thông tin chi tiết của sản phẩm nếu tìm thấy, hoặc null nếu không tìm thấy </returns>
        public async Task<ProductDetailDTO?> GetProductDetailAsync(long productId)
        {
            var product = await _productRepository.GetProductDetailAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            var productDetailDTOs = new ProductDetailDTO
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


        /// <summary>
        /// Phuong thức này dùng để tạo một sản phẩm mới.
        /// </summary>
        /// <param name="productCreationDto"> Truyền vào thông tin sản phẩm mới </param>
        /// <returns name="ProductDetailDTO"> Trả về thông tin chi tiết của sản phẩm mới được tạo </returns>
        /// <exception cref="InvalidOperationException"> Ném ngoại lệ nếu không thể tạo sản phẩm mới </exception>
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

            if (product.StockQuantity == 0)
            {
                product.Status = EProductStatus.OutOfStock;
            }

            var createdProduct = await _productRepository.CreateProductAsync(product);

            if (createdProduct == null)
            {
                throw new InvalidOperationException("Failed to create product.");
            }

            return await GetProductDetailAsync(createdProduct.Id);
        }


        /// <summary>
        /// Phương thức này dùng để cập nhật số lượng tồn kho của một sản phẩm.
        /// </summary>
        /// <param name="productId"> Xác định sản phẩm được cập nhật bằng Id </param>
        /// <param name="updateStockQuantityDto"> Truyền vào số lượng thay đổi </param>
        /// <returns name="ProductDetailDTO"> Trả về thông tin chi tiết của sản phẩm sau khi cập nhật thành công </returns>
        /// <exception cref="KeyNotFoundException"> Ném ngoại lệ nếu không tìm thấy sản phẩm với ID đã cho </exception>
        /// <exception cref="InvalidOperationException"> Ném ngoại lệ nếu số lượng tồn kho không đủ để giảm </exception>
        public async Task<ProductDetailDTO?> UpdateProductStockQuantityAsync(long productId, UpdateStockQuantityDTO updateStockQuantityDto)
        {
            var product = await _productRepository.GetProductDetailAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            if (updateStockQuantityDto.ChangeQuantity < 0) 
            {
                var amountToReduce = - updateStockQuantityDto.ChangeQuantity;
                if (product.StockQuantity < amountToReduce)
                {
                    throw new InvalidOperationException($"Insufficient stock. Cannot reduce by {amountToReduce} when only {product.StockQuantity} are available.");
                }
                if (product.StockQuantity - amountToReduce == 0 && product.Status != EProductStatus.Archived)
                {
                    product.Status = EProductStatus.OutOfStock;
                }
            } 
            else
            {
                if (product.StockQuantity + updateStockQuantityDto.ChangeQuantity > 0 && product.Status != EProductStatus.Archived)
                {
                    product.Status = EProductStatus.Available;
                }
            }

            product.StockQuantity += updateStockQuantityDto.ChangeQuantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateProductAsync(product);

            return await GetProductDetailAsync(productId);
        }


        /// <summary>
        /// Phương thức này dùng để thay đổi status của một sản phẩm thành "Archived", 
        /// sđánh dấu nó là đã bị xóa nhưng vẫn giữ lại trong cơ sở dữ liệu.
        /// </summary>
        /// <param name="productId"> Xác định sản phẩm được lưu trữ bằng Id </param>
        /// <returns name="ProductDetailDTO"> Trả về thông tin chi tiết của sản phẩm sau khi lưu trữ thành công </returns>
        /// <exception cref="KeyNotFoundException"> Ném ngoại lệ nếu không tìm thấy sản phẩm với ID đã cho </exception>
        /// <exception cref="InvalidOperationException"> Ném ngoại lệ nếu sản phẩm đã được lưu trữ trước đó </exception>
        public async Task<ProductDetailDTO?> ArchiveProductAsync(long productId)
        {
            var product = await _productRepository.GetProductDetailAsync(productId);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
            if (product.Status == EProductStatus.Archived)
            {
                throw new InvalidOperationException($"Product with ID {productId} is already archived.");
            }

            product.Status = EProductStatus.Archived;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateProductAsync(product);

            return await GetProductDetailAsync(productId);
        }


        /// <summary>
        /// Phương thức này dùng để cập nhật thông tin chi tiết của một sản phẩm.
        /// </summary>
        /// <param name="productId"> Xác định sản phẩm cần cập nhật bằng Id </param>
        /// <param name="productUpdateDto"> Truyền vào thông tin cập nhật sản phẩm </param>
        /// <returns name="ProductDetailDTO"> Trả về thông tin chi tiết của sản phẩm sau khi cập nhật thành công </returns>
        /// <exception cref="KeyNotFoundException"> Ném ngoại lệ nếu không tìm thấy sản phẩm với ID đã cho </exception>
        public async Task<ProductDetailDTO?> UpdateProductDetailAsync(long productId, ProductUpdateDTO productUpdateDto)
        {
            var product = await _productRepository.GetByIdWithImagesAsync(productId);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;
            product.BrandId = productUpdateDto.BrandId;
            product.CategoryId = productUpdateDto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            var existingImageUrls = product.Images.Select(i => i.ImageUrl).ToList();
            var newImageUrls = productUpdateDto.Images ?? new List<string>();

            var imagesToDelete = product.Images
                .Where(img => !newImageUrls.Contains(img.ImageUrl))
                .ToList();

            foreach (var image in imagesToDelete)
            {
                product.Images.Remove(image);
            }

            var urlsToAdd = newImageUrls
                .Where(url => !existingImageUrls.Contains(url))
                .ToList();

            foreach (var url in urlsToAdd)
            {
                product.Images.Add(new ProductImage { ImageUrl = url });
            }

            await _productRepository.UpdateProductAsync(product);

            return await GetProductDetailAsync(productId);
        }
    }
}
