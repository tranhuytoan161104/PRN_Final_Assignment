using Final.WebApp.DTOs.Orders;
using Final.WebApp.DTOs.Common;
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
    }
}
