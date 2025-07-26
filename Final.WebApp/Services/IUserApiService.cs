using Final.WebApp.DTOs.Users;
using Final.WebApp.DTOs.PasswordReset;
using Final.WebApp.DTOs.Common;

namespace Final.WebApp.Services
{
    public interface IUserApiService
    {
        Task<string> LoginAsync(LoginDTO loginDto);
        Task<UserDTO> RegisterAsync(RegisterDTO registerDto);
        Task<UserProfileDTO> GetMyProfileAsync();
        Task<UserProfileDTO> UpdateMyProfileAsync(UpdateProfileDTO profile);
        Task ChangeMyPasswordAsync(ChangePasswordDTO passwords);

        Task SetupSecurityQuestionAsync(SetupSecurityQuestionDTO dto);
        Task<string> GetSecurityQuestionByEmailAsync(string email);
        Task<string> VerifySecurityAnswerAndGenerateTokenAsync(VerifySecurityAnswerDTO dto);
        Task SendRecoveryEmailAsync(SendRecoveryEmailDTO dto);
        Task ResetPasswordAsync(ResetPasswordDTO dto);
        Task LinkRecoveryEmailAsync(LinkRecoveryEmailDTO dto);

        Task<PagedResult<UserDTO>> GetAllUsersAsync(UserQuery query);
        Task<UserProfileDTO> GetUserByIdAsync(long userId);
        Task UpdateUserStatusAsync(long userId, UpdateUserStatusDTO dto);
        Task UpdateUserRoleAsync(long userId, UserRoleDTO dto);

        Task<List<RecentUserDTO>> GetRecentUsersAsync();
    }
}
