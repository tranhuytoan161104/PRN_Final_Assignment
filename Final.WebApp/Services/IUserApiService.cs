using Final.WebApp.DTOs.Users;

namespace Final.WebApp.Services
{
    public interface IUserApiService
    {
        Task<string> LoginAsync(LoginDTO loginDto);
        Task<UserDTO> RegisterAsync(RegisterDTO registerDto);
    }
}
