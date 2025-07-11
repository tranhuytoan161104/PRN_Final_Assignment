using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.UserAPI.DTOs;

namespace Final.UserAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<UserDTO> RegisterUser(RegisterDTO registerDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"The email {registerDto.Email} already exists");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = "Customer", 
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateUser(newUser);

            return new UserDTO
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                Role = createdUser.Role
            };
        }

        public async Task<TokenDTO> LoginUser(LoginDTO loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid Email or Password.");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid Email or Password.");
            }

            var token = _tokenService.CreateToken(user);

            return new TokenDTO { AccessToken = token };
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(long userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UserProfileDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserProfileDTO?> UpdateUserProfileAsync(long userId, UpdateProfileDTO updateDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;

            await _userRepository.UpdateUserAsync(user);

            return new UserProfileDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> ChangeUserPasswordAsync(long userId, ChangePasswordDTO changePasswordDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var isOldPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, user.PasswordHash);
            if (!isOldPasswordValid)
            {
                throw new InvalidOperationException("Incorrect Old Password.");
            }

            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            user.PasswordHash = newPasswordHash;

            await _userRepository.UpdateUserAsync(user);

            return true;
        }

        public async Task<PagedResult<UserDTO>> GetAllUsersAsync(UserQuery query)
        {
            var pagedResultEntity = await _userRepository.GetAllUserAsync(query);

            var userDtos = pagedResultEntity.Items?.Select(u => new UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role
            }).ToList() ?? new List<UserDTO>();

            return new PagedResult<UserDTO>
            {
                Items = userDtos,
                PageNumber = pagedResultEntity.PageNumber,
                PageSize = pagedResultEntity.PageSize,
                TotalItems = pagedResultEntity.TotalItems,
                TotalPages = pagedResultEntity.TotalPages
            };
        }

        public async Task<UserDTO?> UpdateUserRoleAsync(long userId, UserRoleDTO userRoleDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null; 
            }

            user.Role = userRoleDto.Role;

            await _userRepository.UpdateUserAsync(user);

            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
