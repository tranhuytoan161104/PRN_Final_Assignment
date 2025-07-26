using Final.Domain.Common;
using Final.Domain.Queries;
using Final.UserAPI.DTOs;
using Final.UserAPI.DTOs.PasswordReset;

namespace Final.UserAPI.Services
{
    public interface IUserService
    {
        // --- Nghiệp vụ người dùng cơ bản ---
        Task<UserDTO> RegisterUserAsync(RegisterDTO registerDto);
        Task<TokenDTO> LoginUserAsync(LoginDTO loginDto);
        Task<UserProfileDTO?> GetUserProfileByUserIdAsync(long userId);
        Task<UserProfileDTO?> UpdateUserProfileByUserIdAsync(long userId, UpdateProfileDTO updateDto);
        Task<bool> ChangeUserPasswordByUserIdAsync(long userId, ChangePasswordDTO changePasswordDto);
        Task<PagedResult<UserDTO>> GetAllUsersAsync(UserQuery query);
        Task<UserDTO?> UpdateUserRoleByUserIdAsync(long userId, UserRoleDTO userRoleDto);
        Task<UserDTO?> UpdateUserStatusByUserIdAsync(long userId, UpdateUserStatusDTO updateUserStatusDto);

        // --- Nghiệp vụ Quên mật khẩu & Khôi phục ---
        Task SetupSecurityQuestionAsync(SetupSecurityQuestionDTO dto);
        Task<string> GetSecurityQuestionByEmailAsync(string email);
        Task<string> VerifySecurityAnswerAndGenerateTokenAsync(VerifySecurityAnswerDTO dto);
        Task SendRecoveryEmailAsync(SendRecoveryEmailDTO dto); 
        Task ResetPasswordAsync(ResetPasswordDTO dto);
        Task SendVerificationEmailAsync(long userId, LinkRecoveryEmailDTO dto);
        Task<bool> VerifyRecoveryEmailTokenAsync(long userId, string token);
    }
}
