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

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] ProductQuery query)
        {
            var products = await _productService.GetAllProductsAsync(query);
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductDetail(long productId)
        {
            var product = await _productService.GetProductDetailAsync(productId);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreationDTO productCreationDto)
        {
            var createdProduct = await _productService.CreateProductAsync(productCreationDto);
            return CreatedAtAction(nameof(GetProductDetail), new { productId = createdProduct.Id }, createdProduct);
        }

        [HttpPatch("{productId}/add-stockquantity")]
        [ProducesResponseType(typeof(ProductDetailDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddStockQuantityAsync(long productId, [FromBody] UpdateStockQuantityDTO updateStockQuantityDto)
        {
            var updatedProduct = await _productService.AddProductQuantityAsync(productId, updateStockQuantityDto);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        [HttpPatch("{productId}/reduce-stockquantity")]
        [ProducesResponseType(typeof(ProductDetailDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReduceStockQuantityAsync(long productId, [FromBody] UpdateStockQuantityDTO updateStockQuantityDto)
        {
            var updatedProduct = await _productService.ReduceProductQuantityAsync(productId, updateStockQuantityDto);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(typeof(ProductDetailDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ArchiveProductAsync(long productId)
        {
            var archivedProduct = await _productService.ArchiveProductAsync(productId);
            if (archivedProduct == null)
            {
                return NotFound();
            }
            return Ok(archivedProduct);
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(typeof(ProductDetailDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProductAsync(long productId, [FromBody] ProductUpdateDTO productUpdateDto)
        {
            var updatedProduct = await _productService.UpdateProductAsync(productId, productUpdateDto);

            if (updatedProduct == null)
            {
                return NotFound(); 
            }

            return Ok(updatedProduct);
        }
    }
}
