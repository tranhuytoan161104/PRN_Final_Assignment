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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _productService.GetAllProductsAsync(pageNumber, pageSize);
            return Ok(products);
        }

        [HttpGet("by_category")]
        public async Task<IActionResult> GetProductsByCategoryAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] long categoryId = 1)
        {
            var products = await _productService.GetProductsByCategoryAsync(pageNumber, pageSize, categoryId);
            return Ok(products);
        }
    }
}
