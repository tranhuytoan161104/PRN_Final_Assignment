using Final.WebApp.DTOs.Common;
using Final.WebApp.DTOs.Orders;
using System.Net;
using System.Text.Json;
using System.Web;

namespace Final.WebApp.Services
{
    public class OrderApiService : IOrderApiService
    {
        private readonly HttpClient _httpClient;
        public OrderApiService(HttpClient httpClient) { _httpClient = httpClient; }

        public async Task<List<PaymentMethodDTO>> GetPaymentMethodsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PaymentMethodDTO>>("api/payment-methods") ?? [];
        }

        public async Task<OrderDTO> CreateOrderAsync(CreateOrderDTO createOrderDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/orders", createOrderDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderDTO>() ?? throw new InvalidOperationException();
            }

            var errorContent = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            throw new HttpRequestException(errorContent?["message"]);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(long orderId)
        {
            return await _httpClient.GetFromJsonAsync<OrderDTO>($"api/orders/{orderId}") ?? throw new KeyNotFoundException();
        }

        public async Task<PagedResult<OrderDTO>> GetMyOrdersAsync(OrderQuery query)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["pageNumber"] = query.PageNumber.ToString();
            queryString["pageSize"] = query.PageSize.ToString();

            var response = await _httpClient.GetAsync($"api/orders?{queryString}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PagedResult<OrderDTO>>() ?? new PagedResult<OrderDTO>();
        }

        public async Task<OrderDTO> CancelOrderAsync(long orderId)
        {
            var response = await _httpClient.PatchAsync($"api/orders/{orderId}/cancel", null);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderDTO>() ?? throw new InvalidOperationException();
            }

            var errorContent = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            throw new HttpRequestException(errorContent?["message"]);
        }

        public async Task<PagedResult<OrderDTO>> GetAllOrdersAsync(OrderQuery query)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["pageNumber"] = query.PageNumber.ToString();
            queryString["pageSize"] = query.PageSize.ToString();

            var response = await _httpClient.GetAsync($"api/orders/admin/all?{queryString}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PagedResult<OrderDTO>>() ?? new();
        }

        public async Task<OrderDTO> GetAdminOrderDetailAsync(long orderId)
        {
            return await _httpClient.GetFromJsonAsync<OrderDTO>($"api/orders/admin/{orderId}")
                ?? throw new KeyNotFoundException("Không tìm thấy đơn hàng.");
        }

        public async Task UpdateOrderStatusAsync(long orderId, UpdateOrderStatusDTO dto)
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/orders/admin/{orderId}/status", dto);
            if (!response.IsSuccessStatusCode)
            {
                await HandleErrorResponse(response);
            }
        }

        private async Task HandleErrorResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpRequestException("Phiên đăng nhập hết hạn hoặc không hợp lệ.", null, response.StatusCode);
            }

            var errorJsonString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(errorJsonString))
            {
                throw new HttpRequestException($"Yêu cầu không thành công. Mã trạng thái: {response.StatusCode}", null, response.StatusCode);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                try
                {
                    var validationProblem = JsonSerializer.Deserialize<ValidationProblemDTO>(errorJsonString);
                    if (validationProblem != null && validationProblem.Errors.Any())
                    {
                        var errorMessages = validationProblem.Errors.SelectMany(e => e.Value);
                        throw new HttpRequestException(string.Join("\n", errorMessages), null, response.StatusCode);
                    }
                }
                catch (JsonException) { }
            }

            try
            {
                var errorContent = JsonSerializer.Deserialize<Dictionary<string, string>>(errorJsonString);
                if (errorContent != null && errorContent.TryGetValue("message", out var message))
                {
                    throw new HttpRequestException(message, null, response.StatusCode);
                }
            }
            catch (JsonException) { }

            throw new HttpRequestException($"Yêu cầu không thành công. Mã trạng thái: {response.StatusCode}", null, response.StatusCode);
        }
    }
}
