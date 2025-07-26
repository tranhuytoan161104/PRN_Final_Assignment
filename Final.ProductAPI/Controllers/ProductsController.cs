using Final.Domain.Common;
using Final.Domain.Queries;
using Final.ProductAPI.DTOs;
using Final.ProductAPI.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResult<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductDTO>>> GetAllProductsAsync([FromQuery] ProductQuery query)
        {
            var products = await _productService.GetAllProductsAsync(query);
            return Ok(products);
        }

        [HttpGet("{productId:long}", Name = "GetProductDetailByProductIdAsync")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ProductDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDTO>> GetProductDetailByProductIdAsync(long productId)
        {
            var product = await _productService.GetProductDetailByProductIdAsync(productId);
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductDetailDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ProductDetailDTO>> CreateProductAsync([FromBody] ProductCreationDTO productCreationDto)
        {
            var createdProduct = await _productService.CreateProductAsync(productCreationDto);
            return CreatedAtRoute("GetProductDetailByProductIdAsync", new { productId = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{productId:long}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDTO>> UpdateProductAsync(long productId, [FromBody] ProductUpdateDTO productUpdateDto)
        {
            var updatedProduct = await _productService.UpdateProductDetailAsync(productId, productUpdateDto);
            return Ok(updatedProduct);
        }

        [HttpPatch("{productId:long}/stock-quantity")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDTO>> UpdateProductStockQuantityAsync(long productId, [FromBody] StockQuantityUpdateDTO updateStockQuantityDto)
        {
            var updatedProduct = await _productService.UpdateProductStockQuantityAsync(productId, updateStockQuantityDto);
            return Ok(updatedProduct);
        }

        [HttpPatch("{productId:long}/archive")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ProductDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDetailDTO>> ArchiveProductAsync(long productId)
        {
            var archivedProduct = await _productService.ArchiveProductAsync(productId);
            return Ok(archivedProduct);
        }
    }
}