using Final.Domain.Common;
using Final.Domain.Queries;
using Final.UserAPI.DTOs;

namespace Final.UserAPI.Services
{
    public interface IUserService
    {
        Task<UserDTO> RegisterUser(RegisterDTO registerDto);
        Task<TokenDTO> LoginUser(LoginDTO loginDto);
        Task<UserProfileDTO?> GetUserProfileAsync(long userId);
        Task<UserProfileDTO?> UpdateUserProfileAsync(long userId, UpdateProfileDTO updateDto);
        Task<bool> ChangeUserPasswordAsync(long userId, ChangePasswordDTO changePasswordDto);
        Task<PagedResult<UserDTO>> GetAllUsersAsync(UserQuery query);
        Task<UserDTO?> UpdateUserRoleAsync(long userId, UserRoleDTO userRoleDto);
        Task<UserDTO?> UpdateUserStatusAsync(long userId, UpdateUserStatusDTO updateUserStatusDto);
    }
}
