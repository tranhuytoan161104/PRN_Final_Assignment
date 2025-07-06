using Final.Domain.Entities;
using Final.Domain.Queries;
using Final.ProductAPI.DTOs;
using Final.ProductAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        /// <summary>
        /// Phuong thức này dùng để lấy danh sách sản phẩm với phân trang và lọc theo các tiêu chí.
        /// </summary>
        /// <param name="query"> Truyền các thông tin phân trang và lọc sản phẩm từ request body </param>
        /// <returns name="Ok"> Trả về danh sách sản phẩm đã được phân trang và lọc </returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] ProductQuery query)
        {
            var products = await _productService.GetAllProductsAsync(query);
            return Ok(products);
        }


        /// <summary>
        /// Phương thức này dùng để lấy thông tin chi tiết của một sản phẩm dựa trên Id.
        /// </summary>
        /// <param name="productId"> Xác định sản phẩm cần lấy thông tin bằng Id </param>
        /// <returns name="Ok"> Trả về thông tin chi tiết của sản phẩm nếu tìm thấy </returns>
        /// <returns name="NotFound"> Trả về 404 và thông báo khi không tìm thấy sản phẩm </returns>
        /// <returns name="StatusCode"> Trả về 500 và thông báo khi có lỗi không mong muốn xảy ra </returns>
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductDetail(long productId)
        {
            try
            {
                var product = await _productService.GetProductDetailAsync(productId);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        /// <summary>
        /// Phương thức này dùng để tạo một sản phẩm mới.
        /// </summary>
        /// <param name="productCreationDto"> Truyền vào thông tin sản phẩm mới từ request body </param>
        /// <returns name="CreatedAtAction"> Trả về thông tin chi tiết của sản phẩm mới được tạo </returns>
        /// <returns name="BadRequest"> Trả về 400 và thông báo khi có lỗi trong quá trình tạo sản phẩm </returns>
        /// <returns name="StatusCode"> Trả về 500 và thông báo khi có lỗi không mong muốn xảy ra </returns>
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreationDTO productCreationDto)
        {
            try
            {
                var createdProduct = await _productService.CreateProductAsync(productCreationDto);
                return CreatedAtAction(nameof(GetProductDetail), new { productId = createdProduct.Id }, createdProduct);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        /// <summary>
        /// Phương thức này dùng để cập nhật số lượng tồn kho của một sản phẩm.
        /// </summary>
        /// <param name="productId"> Xác định sản phẩm được cập nhật bằng Id </param>
        /// <param name="updateStockQuantityDto"> Truyền vào số lượng thay đổi từ request body </param>
        /// <returns name="Ok"> Trả về thông tin chi tiết của sản phẩm sau khi cập nhật thành công </returns>
        /// <returns name="NotFound"> Trả về 404 và thông báo khi không tìm thấy sản phẩm </returns>
        /// <returns name="BadRequest"> Trả về 400 và thông báo khi có lỗi trong quá trình cập nhật </returns>
        /// <returns name="StatusCode"> Trả về 500 và thông báo khi có lỗi không mong muốn xảy ra </returns>
        [HttpPatch("{productId}/update-stockquantity")]
        public async Task<IActionResult> UpdateProductStockQuantityAsync(long productId, [FromBody] UpdateStockQuantityDTO updateStockQuantityDto)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductStockQuantityAsync(productId, updateStockQuantityDto);
                return Ok(updatedProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        /// <summary>
        /// Phương thức này dùng để lưu trữ một sản phẩm, đánh dấu nó là đã bị xóa nhưng vẫn giữ lại trong cơ sở dữ liệu.
        /// </summary>
        /// <param name="productId"> Xác định sản phẩm được lưu trữ bằng Id </param>
        /// <returns name="Ok"> Trả về thông tin chi tiết của sản phẩm đã được lưu trữ </returns>
        /// <returns name="NotFound"> Trả về 404 và thông báo khi không tìm thấy sản phẩm </returns>
        /// <returns name="BadRequest"> Trả về 400 và thông báo khi có lỗi trong quá trình lưu trữ </returns>
        /// <returns name="StatusCode"> Trả về 500 và thông báo khi có lỗi không mong muốn xảy ra </returns>
        [HttpPatch("{productId}")]
        public async Task<IActionResult> ArchiveProductAsync(long productId)
        {
            try
            {
                var archivedProduct = await _productService.ArchiveProductAsync(productId);
                return Ok(archivedProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        /// <summary>
        /// Phương thức này dùng để cập nhật thông tin bất kỳ của một sản phẩm.
        /// </summary>
        /// <param name="productId"> Xác định sản phẩm được cập nhật bằng Id </param>
        /// <param name="productUpdateDto"> Truyền vào thông tin cập nhật sản phẩm từ request body </param>
        /// <returns name="Ok"> Trả về thông tin chi tiết của sản phẩm sau khi cập nhật thành công </returns>
        /// <returns name="NotFound"> Trả về 404 và thông báo khi không tìm thấy sản phẩm </returns>
        /// <returns name="StatusCode"> Trả về 500 và thông báo khi có lỗi không mong muốn xảy ra </returns>
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProductAsync(long productId, [FromBody] ProductUpdateDTO productUpdateDto)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductDetailAsync(productId, productUpdateDto);
                return Ok(updatedProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
