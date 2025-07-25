using Final.Domain.Common;
using Final.Domain.Queries;
using Final.UserAPI.DTOs;

namespace Final.UserAPI.Services
{
    public interface IUserService
    {
        Task<UserDTO> RegisterUserAsync(RegisterDTO registerDto);
        Task<TokenDTO> LoginUserAsync(LoginDTO loginDto);
        Task<UserProfileDTO?> GetUserProfileByUserIdAsync(long userId);
        Task<UserProfileDTO?> UpdateUserProfileByUserIdAsync(long userId, UpdateProfileDTO updateDto);
        Task<bool> ChangeUserPasswordByUserIdAsync(long userId, ChangePasswordDTO changePasswordDto);
        Task<PagedResult<UserDTO>> GetAllUsersAsync(UserQuery query);
        Task<UserDTO?> UpdateUserRoleByUserIdAsync(long userId, UserRoleDTO userRoleDto);
        Task<UserDTO?> UpdateUserStatusByUserIdAsync(long userId, UpdateUserStatusDTO updateUserStatusDto);
    }
}
