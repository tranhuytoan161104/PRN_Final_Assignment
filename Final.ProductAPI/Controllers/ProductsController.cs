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
        public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _productService.GetAllProductsAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}
