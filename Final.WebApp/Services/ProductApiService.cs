using Final.WebApp.DTOs.Common;
using Final.WebApp.DTOs.Products;
using System.Text.Json;
using System.Web;

namespace Final.WebApp.Services
{
    public class ProductApiService : IProductApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductApiService> _logger;

        public ProductApiService(HttpClient httpClient, ILogger<ProductApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<CategoryDTO>>("api/categories") ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách danh mục từ API.");
                return [];
            }
        }

        public async Task<List<BrandDTO>> GetBrandsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<BrandDTO>>("api/brands") ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách thương hiệu từ API.");
                return [];
            }
        }

        public async Task<PagedResult<ProductDTO>> GetProductsAsync(ProductQuery query)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["pageNumber"] = query.PageNumber.ToString();
            queryString["pageSize"] = query.PageSize.ToString();

            if (query.CategoryId.HasValue) queryString["categoryId"] = query.CategoryId.Value.ToString();
            if (query.BrandId.HasValue) queryString["brandId"] = query.BrandId.Value.ToString();
            if (query.MinPrice.HasValue) queryString["minPrice"] = query.MinPrice.Value.ToString();
            if (query.MaxPrice.HasValue) queryString["maxPrice"] = query.MaxPrice.Value.ToString();
            if (!string.IsNullOrEmpty(query.Name)) queryString["name"] = query.Name;
            if (!string.IsNullOrEmpty(query.SortBy)) queryString["sortBy"] = query.SortBy;
            if (!string.IsNullOrEmpty(query.SortDirection)) queryString["sortDirection"] = query.SortDirection;

            var response = await _httpClient.GetAsync($"api/products?{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<ProductDTO>>();
                return pagedResult ?? new PagedResult<ProductDTO>();
            }

            _logger.LogError("Lỗi khi gọi ProductAPI. Status code: {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Lỗi truy vấn sản phẩm. Máy chủ API phản hồi với mã: {response.StatusCode}");
        }

        public async Task<ProductDetailDTO> GetProductDetailAsync(long productId)
        {
            var response = await _httpClient.GetAsync($"api/products/{productId}");

            if (response.IsSuccessStatusCode)
            {
                var productDetail = await response.Content.ReadFromJsonAsync<ProductDetailDTO>();
                return productDetail ?? throw new KeyNotFoundException("Không thể phân tích dữ liệu sản phẩm.");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID {productId}.");
            }

            _logger.LogError("Lỗi khi gọi ProductAPI. Status code: {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Lỗi truy vấn chi tiết sản phẩm. Mã lỗi: {response.StatusCode}");
        }
    }
}
