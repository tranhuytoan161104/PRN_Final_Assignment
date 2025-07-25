using System.Text;
using System.Text.Json;
using Final.WebApp.DTOs.Users;

namespace Final.WebApp.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(loginDto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/users/login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var tokenDto = await response.Content.ReadFromJsonAsync<TokenDTO>();
                return tokenDto?.AccessToken ?? string.Empty;
            }

            // Nếu thất bại, đọc thông báo lỗi từ API và ném ra exception
            var errorContent = await response.Content.ReadAsStringAsync();
            var errorDoc = JsonDocument.Parse(errorContent);
            var errorMessage = errorDoc.RootElement.GetProperty("message").GetString();

            throw new HttpRequestException(errorMessage ?? "Thông tin đăng nhập không chính xác.");
        }

        public async Task<UserDTO> RegisterAsync(RegisterDTO registerDto)
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(registerDto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/users/register", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var userDto = await response.Content.ReadFromJsonAsync<UserDTO>();
                return userDto ?? throw new InvalidOperationException("Không thể phân tích dữ liệu người dùng trả về.");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            var errorDoc = JsonDocument.Parse(errorContent);
            var errorMessage = errorDoc.RootElement.GetProperty("message").GetString();

            throw new HttpRequestException(errorMessage ?? "Đăng ký không thành công.");
        }
    }
}
