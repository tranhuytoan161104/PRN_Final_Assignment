using Final.ProductAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Final.ProductAPI.Services;

namespace Final.ProductAPI.Controllers
{
    [Route("api/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BrandDTO>>> GetAllBrandsAsync()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }
    }
}
