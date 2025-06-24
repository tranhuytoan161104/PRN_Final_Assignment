using Final.Domain.Queries;
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

        [HttpGet("detail")]
        public async Task<IActionResult> GetProductDetail([FromQuery] long productId)
        {
            var product = await _productService.GetProductDetailAsync(productId);
            if (product == null) return NotFound();
            return Ok(product);
        }
    }
}
