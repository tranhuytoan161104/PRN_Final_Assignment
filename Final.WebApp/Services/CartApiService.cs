using Final.WebApp.DTOs.Carts;
using Final.WebApp.DTOs.Common;
using System.Net;
using System.Text.Json;

namespace Final.WebApp.Services
{
    public class CartApiService : ICartApiService
    {
        private readonly HttpClient _httpClient;
        public CartApiService(HttpClient httpClient) { _httpClient = httpClient; }

        public async Task<CartDTO> GetCartAsync()
        {
            var response = await _httpClient.GetAsync("api/cart");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartDTO>() ?? new CartDTO();
            }
            await HandleErrorResponse(response); 
            return new CartDTO(); 
        }

        public async Task<CartDTO> AddItemToCartAsync(AddCartItemDTO item)
        {
            var response = await _httpClient.PostAsJsonAsync("api/cart/items", item);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartDTO>() ?? new CartDTO();
            }
            await HandleErrorResponse(response);
            return new CartDTO();
        }

        public async Task<CartDTO> UpdateItemQuantityAsync(long productId, int quantity)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/cart/items/{productId}", new { quantity });
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartDTO>() ?? new CartDTO();
            }
            await HandleErrorResponse(response);
            return new CartDTO();
        }

        public async Task<CartDTO> RemoveItemFromCartAsync(long productId)
        {
            var response = await _httpClient.DeleteAsync($"api/cart/items/{productId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartDTO>() ?? new CartDTO();
            }
            await HandleErrorResponse(response);
            return new CartDTO();
        }

        private async Task HandleErrorResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpRequestException("Bạn cần đăng nhập để thực hiện hành động này.", null, response.StatusCode);
            }

            if (response.Content.Headers.ContentLength == 0)
            {
                throw new HttpRequestException($"Yêu cầu không thành công. Mã trạng thái: {response.StatusCode}", null, response.StatusCode);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var validationProblem = await response.Content.ReadFromJsonAsync<ValidationProblemDTO>();
                if (validationProblem != null && validationProblem.Errors.Any())
                {
                    var errorMessages = validationProblem.Errors.SelectMany(e => e.Value);
                    throw new HttpRequestException(string.Join("\n", errorMessages), null, response.StatusCode);
                }
            }

            try
            {
                var errorContent = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var message = errorContent?["message"];
                throw new HttpRequestException(message, null, response.StatusCode);
            }
            catch (JsonException) 
            {
                throw new HttpRequestException($"Yêu cầu không thành công. Mã trạng thái: {response.StatusCode}", null, response.StatusCode);
            }
        }
    }
}
