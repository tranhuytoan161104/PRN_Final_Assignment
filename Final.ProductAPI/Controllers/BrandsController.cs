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

        /// <summary>
        /// Lấy danh sách tất cả các thương hiệu.
        /// </summary>
        /// <returns>Danh sách các thương hiệu.</returns>
        [HttpGet]
        public async Task<ActionResult<List<BrandDTO>>> GetAllBrandsAsync()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }
    }
}
