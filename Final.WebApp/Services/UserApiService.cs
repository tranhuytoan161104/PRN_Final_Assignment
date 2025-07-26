using Final.WebApp.DTOs.Common;
using Final.WebApp.DTOs.Users;
using Final.WebApp.DTOs.PasswordReset;
using System.Net;
using System.Text;
using System.Text.Json;

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

        public async Task<UserProfileDTO> GetMyProfileAsync()
        {
            return await _httpClient.GetFromJsonAsync<UserProfileDTO>("api/users/profile")
                ?? throw new HttpRequestException("Không thể tải được hồ sơ người dùng.");
        }

        public async Task<UserProfileDTO> UpdateMyProfileAsync(UpdateProfileDTO profile)
        {
            var response = await _httpClient.PutAsJsonAsync("api/users/profile", profile);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserProfileDTO>() ?? throw new InvalidOperationException();
            }
            await HandleErrorResponse(response); 
            return null!; 
        }

        public async Task ChangeMyPasswordAsync(ChangePasswordDTO passwords)
        {
            var response = await _httpClient.PatchAsJsonAsync("api/users/change-password", passwords);
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

            // Đọc nội dung lỗi MỘT LẦN DUY NHẤT và lưu vào biến
            var errorJsonString = await response.Content.ReadAsStringAsync();

            // Nếu không có nội dung, ném lỗi chung chung
            if (string.IsNullOrEmpty(errorJsonString))
            {
                throw new HttpRequestException($"Yêu cầu không thành công. Mã trạng thái: {response.StatusCode}", null, response.StatusCode);
            }

            // Nếu là lỗi 400 Bad Request, rất có thể là lỗi validation
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
                catch (JsonException) { /* Bỏ qua nếu không phải định dạng validation */ }
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

        public async Task SetupSecurityQuestionAsync(SetupSecurityQuestionDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/setup-security-question", dto);
            if (!response.IsSuccessStatusCode) await HandleErrorResponse(response);
        }

        public async Task<string> GetSecurityQuestionByEmailAsync(string email)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/forgot-password/question", new { email });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                return result.GetProperty("question").GetString() ?? string.Empty;
            }
            await HandleErrorResponse(response);
            return string.Empty;
        }

        public async Task<string> VerifySecurityAnswerAndGenerateTokenAsync(VerifySecurityAnswerDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/forgot-password/verify-answer", dto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                return result.GetProperty("resetToken").GetString() ?? string.Empty;
            }
            await HandleErrorResponse(response);
            return string.Empty;
        }

        public async Task SendRecoveryEmailAsync(SendRecoveryEmailDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/forgot-password/send-email", dto);
            if (!response.IsSuccessStatusCode) await HandleErrorResponse(response);
        }

        public async Task ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/reset-password", dto);
            if (!response.IsSuccessStatusCode) await HandleErrorResponse(response);
        }

        public async Task LinkRecoveryEmailAsync(LinkRecoveryEmailDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users/profile/link-recovery-email", dto);
            if (!response.IsSuccessStatusCode) await HandleErrorResponse(response);
        }
    }
}
