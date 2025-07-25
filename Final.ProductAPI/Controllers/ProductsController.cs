using Final.Domain.Common;
using Final.Domain.Queries;
using Final.ProductAPI.DTOs;
using Final.ProductAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm với phân trang và lọc theo nhiều tiêu chí.
        /// Cho phép người dùng lọc sản phẩm theo danh mục, thương hiệu, tên, giá và sắp xếp theo các trường khác nhau.
        /// </summary>
        /// <param name="query">Chứa các thông tin phân trang và lọc sản phẩm.</param>
        /// <returns>Một danh sách sản phẩm đã được phân trang và lọc.</returns>
        /// <response code="200">Trả về danh sách sản phẩm thành công.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductDTO>>> GetAllProductsAsync([FromQuery] ProductQuery queries)
        {
            var products = await _productService.GetAllProductsAsync(queries);
            return Ok(products);
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một sản phẩm dựa trên Id.
        /// Cho phép người dùng xem thông tin chi tiết của sản phẩm bao gồm mô tả, giá, hình ảnh và các thuộc tính khác.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần lấy thông tin.</param>
        /// <returns>Thông tin chi tiết của sản phẩm nếu tìm thấy.</returns>
        /// <response code="200">Trả về thông tin chi tiết của sản phẩm.</response>
        /// <response code="404">Nếu không tìm thấy sản phẩm với ID tương ứng.</response>
        [HttpGet("{productId:long}", Name = "GetProductDetailByIdAsync")]
        [ProducesResponseType(typeof(ProductDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDTO>> GetProductDetailByIdAsync(long productId)
        {
            var product = await _productService.GetProductDetailByIdAsync(productId);
            return Ok(product);
        }

        /// <summary>
        /// Tạo một sản phẩm mới.
        /// Cho phép người dùng thêm sản phẩm mới vào hệ thống với các thông tin như tên, mô tả, giá, hình ảnh và các thuộc tính khác.
        /// </summary>
        /// <param name="productCreationDto">Thông tin sản phẩm cần tạo.</param>
        /// <returns>Thông tin chi tiết của sản phẩm vừa được tạo.</returns>
        /// <response code="201">Trả về sản phẩm vừa được tạo cùng với URL để truy cập.</response>
        /// <response code="400">Nếu dữ liệu đầu vào không hợp lệ.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDetailDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDetailDTO>> CreateProductAsync([FromBody] ProductCreationDTO productCreationDto)
        {
            var createdProduct = await _productService.CreateProductAsync(productCreationDto);
            return CreatedAtRoute("GetProductDetailByIdAsync", new { productId = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Cập nhật toàn bộ thông tin chi tiết của một sản phẩm.
        /// Cho phép người dùng cập nhật các thông tin như tên, mô tả, giá, hình ảnh và các thuộc tính khác của sản phẩm.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần cập nhật.</param>
        /// <param name="productUpdateDto">Thông tin mới của sản phẩm.</param>
        /// <returns>Thông tin sản phẩm sau khi cập nhật thành công.</returns>
        /// <response code="200">Trả về thông tin sản phẩm đã được cập nhật.</response>
        /// <response code="400">Nếu dữ liệu đầu vào không hợp lệ.</response>
        /// <response code="404">Nếu không tìm thấy sản phẩm với ID tương ứng.</response>
        [HttpPut("{productId:long}")]
        [ProducesResponseType(typeof(ProductDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDTO>> UpdateProductAsync(long productId, [FromBody] ProductUpdateDTO productUpdateDto)
        {
            var updatedProduct = await _productService.UpdateProductDetailAsync(productId, productUpdateDto);
            return Ok(updatedProduct);
        }

        /// <summary>
        /// Cập nhật số lượng tồn kho của một sản phẩm.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần cập nhật.</param>
        /// <param name="updateStockQuantityDto">Số lượng cần thay đổi.</param>
        /// <returns>Thông tin sản phẩm sau khi cập nhật thành công.</returns>
        /// <response code="200">Trả về thông tin sản phẩm đã được cập nhật.</response>
        /// <response code="400">Nếu yêu cầu không hợp lệ (ví dụ: giảm số lượng nhiều hơn hiện có).</response>
        /// <response code="404">Nếu không tìm thấy sản phẩm với ID tương ứng.</response>
        [HttpPatch("{productId:long}/update-stockquantity")]
        [ProducesResponseType(typeof(ProductDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDTO>> UpdateProductStockQuantityAsync(long productId, [FromBody] StockQuantityUpdateDTO updateStockQuantityDto)
        {
            var updatedProduct = await _productService.UpdateProductStockQuantityAsync(productId, updateStockQuantityDto);
            return Ok(updatedProduct);
        }

        /// <summary>
        /// Lưu trữ một sản phẩm (đánh dấu là đã xóa nhưng không xóa khỏi DB).
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần lưu trữ.</param>
        /// <returns>Thông tin sản phẩm sau khi lưu trữ thành công.</returns>
        /// <response code="200">Trả về thông tin sản phẩm đã được lưu trữ.</response>
        /// <response code="400">Nếu sản phẩm đã được lưu trữ từ trước.</response>
        /// <response code="404">Nếu không tìm thấy sản phẩm với ID tương ứng.</response>
        [HttpPatch("{productId:long}/archive")]
        [ProducesResponseType(typeof(ProductDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDTO>> ArchiveProductAsync(long productId)
        {
            var archivedProduct = await _productService.ArchiveProductAsync(productId);
            return Ok(archivedProduct);
        }
    }
}
