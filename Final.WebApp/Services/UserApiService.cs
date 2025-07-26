using Final.WebApp.DTOs.Common;
using Final.WebApp.DTOs.PasswordReset;
using Final.WebApp.DTOs.Users;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

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

        public async Task<PagedResult<UserDTO>> GetAllUsersAsync(UserQuery query)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["pageNumber"] = query.PageNumber.ToString();
            queryString["pageSize"] = query.PageSize.ToString();
            if (!string.IsNullOrEmpty(query.SearchTerm)) queryString["searchTerm"] = query.SearchTerm;
            if (!string.IsNullOrEmpty(query.Role)) queryString["role"] = query.Role;
            if (!string.IsNullOrEmpty(query.Status)) queryString["status"] = query.Status;

            var response = await _httpClient.GetAsync($"api/users?{queryString}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PagedResult<UserDTO>>() ?? new();
        }

        public async Task<UserProfileDTO> GetUserByIdAsync(long userId)
        {
            return await _httpClient.GetFromJsonAsync<UserProfileDTO>($"api/users/{userId}")
                ?? throw new KeyNotFoundException("Không tìm thấy người dùng.");
        }

        public async Task UpdateUserStatusAsync(long userId, UpdateUserStatusDTO dto)
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/users/{userId}/status", dto);
            if (!response.IsSuccessStatusCode) await HandleErrorResponse(response);
        }

        public async Task UpdateUserRoleAsync(long userId, UserRoleDTO dto)
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/users/{userId}/role", dto);
            if (!response.IsSuccessStatusCode) await HandleErrorResponse(response);
        }

        public async Task<List<RecentUserDTO>> GetRecentUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RecentUserDTO>>("api/users/recent") ?? [];
        }
    }
}
