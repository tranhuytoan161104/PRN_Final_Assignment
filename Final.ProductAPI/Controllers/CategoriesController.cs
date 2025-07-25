using Final.ProductAPI.DTOs;
using Final.ProductAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Lấy tất cả các danh mục sản phẩm.
        /// </summary>
        /// <returns>Một danh sách các danh mục.</returns>
        /// <response code="200">Trả về danh sách các danh mục thành công.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryDTO>>> GetAllCategoriesAsync()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return Ok(result);
        }
    }
}
