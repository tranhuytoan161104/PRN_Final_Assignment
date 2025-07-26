using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Enums;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.ProductAPI.DTOs;
using Final.UserAPI.DTOs;
using Final.UserAPI.DTOs.PasswordReset;
using System.Security.Cryptography;

namespace Final.UserAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<UserDTO> RegisterUserAsync(RegisterDTO registerDto)
        {
            if (await _userRepository.GetUserByEmailAsync(registerDto.Email) != null)
            {
                throw new InvalidOperationException($"Email '{registerDto.Email}' đã tồn tại.");
            }

            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "Customer",
                Status = EUserStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateUser(newUser);

            return MapToUserDTO(createdUser);
        }

        public async Task<TokenDTO> LoginUserAsync(LoginDTO loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không chính xác.");
            }

            if (user.Status == EUserStatus.Inactive)
            {
                throw new UnauthorizedAccessException("Tài khoản của bạn đã bị khóa.");
            }

            var token = _tokenService.CreateToken(user);
            return new TokenDTO { AccessToken = token };
        }

        public async Task<UserProfileDTO?> GetUserProfileByUserIdAsync(long userId)
        {
            var user = await _userRepository.GetUserByUserIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");
            }

            return new UserProfileDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserProfileDTO?> UpdateUserProfileByUserIdAsync(long userId, UpdateProfileDTO updateDto)
        {
            var user = await _userRepository.GetUserByUserIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");
            }

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            await _userRepository.UpdateUserAsync(user);

            return await GetUserProfileByUserIdAsync(userId);
        }

        public async Task<bool> ChangeUserPasswordByUserIdAsync(long userId, ChangePasswordDTO changePasswordDto)
        {
            var user = await _userRepository.GetUserByUserIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");
            }

            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, user.PasswordHash))
            {
                throw new InvalidOperationException("Mật khẩu cũ không chính xác.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<PagedResult<UserDTO>> GetAllUsersAsync(UserQuery query)
        {
            var pagedResultEntity = await _userRepository.GetAllUserAsync(query);
            var userDtos = pagedResultEntity.Items?.Select(MapToUserDTO).ToList() ?? new List<UserDTO>();

            return new PagedResult<UserDTO>
            {
                Items = userDtos,
                PageNumber = pagedResultEntity.PageNumber,
                PageSize = pagedResultEntity.PageSize,
                TotalItems = pagedResultEntity.TotalItems,
                TotalPages = pagedResultEntity.TotalPages
            };
        }

        public async Task<UserDTO?> UpdateUserRoleByUserIdAsync(long userId, UserRoleDTO userRoleDto)
        {
            var user = await _userRepository.GetUserByUserIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");
            }

            user.Role = userRoleDto.Role;
            await _userRepository.UpdateUserAsync(user);
            return MapToUserDTO(user);
        }

        public async Task<UserDTO?> UpdateUserStatusByUserIdAsync(long userId, UpdateUserStatusDTO updateUserStatusDto)
        {
            var user = await _userRepository.GetUserByUserIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy người dùng với ID {userId}.");
            }

            user.Status = updateUserStatusDto.Status;
            await _userRepository.UpdateUserAsync(user);
            return MapToUserDTO(user);
        }

        private UserDTO MapToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task SetupSecurityQuestionAsync(SetupSecurityQuestionDTO dto)
        {
            var user = await _userRepository.GetUserByUserIdAsync(dto.UserId)
                ?? throw new KeyNotFoundException("Không tìm thấy người dùng.");

            user.SecurityQuestion = dto.Question;
            user.SecurityAnswerHash = BCrypt.Net.BCrypt.HashPassword(dto.Answer);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task<string> GetSecurityQuestionByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email)
                ?? throw new KeyNotFoundException("Email không tồn tại trong hệ thống.");
            if (string.IsNullOrEmpty(user.SecurityQuestion))
                throw new InvalidOperationException("Người dùng này chưa thiết lập câu hỏi bảo mật.");
            return user.SecurityQuestion;
        }

        public async Task<string> VerifySecurityAnswerAndGenerateTokenAsync(VerifySecurityAnswerDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email)
                ?? throw new KeyNotFoundException("Email không tồn tại.");

            if (string.IsNullOrEmpty(user.SecurityAnswerHash) || !BCrypt.Net.BCrypt.Verify(dto.Answer, user.SecurityAnswerHash))
            {
                throw new InvalidOperationException("Câu trả lời bảo mật không chính xác.");
            }
            var token = GenerateSecureToken();
            user.PasswordResetToken = token;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateUserAsync(user);
            return token;
        }

        public async Task SendRecoveryEmailAsync(SendRecoveryEmailDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email)
                ?? throw new KeyNotFoundException("Email không tồn tại.");
            if (!user.IsRecoveryEmailVerified || string.IsNullOrEmpty(user.RecoveryEmail))
            {
                throw new InvalidOperationException("Tài khoản này chưa liên kết hoặc chưa xác thực email khôi phục.");
            }
            var token = GenerateSecureToken();
            user.PasswordResetToken = token;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateUserAsync(user);
            var resetLink = $"{dto.ResetPasswordUrl}?token={token}&email={Uri.EscapeDataString(user.Email)}";
            var body = $"<p>Vui lòng nhấp vào liên kết sau để đặt lại mật khẩu của bạn:</p><a href='{resetLink}'>Đặt lại mật khẩu</a>";
            await _emailService.SendEmailAsync(user.RecoveryEmail, "Yêu cầu đặt lại mật khẩu", body);
        }

        public async Task ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email)
                ?? throw new KeyNotFoundException("Email không tồn tại.");
            if (user.PasswordResetToken != dto.Token || user.ResetTokenExpiry <= DateTime.UtcNow)
            {
                throw new InvalidOperationException("Token không hợp lệ hoặc đã hết hạn.");
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpiry = null;
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task SendVerificationEmailAsync(long userId, LinkRecoveryEmailDTO dto)
        {
            var user = await _userRepository.GetUserByUserIdAsync(userId)
                 ?? throw new KeyNotFoundException("Không tìm thấy người dùng.");
            var token = GenerateSecureToken();
            user.PasswordResetToken = token;
            user.ResetTokenExpiry = DateTime.UtcNow.AddDays(1);
            user.PendingRecoveryEmail = dto.RecoveryEmail;
            await _userRepository.UpdateUserAsync(user);
            var verificationLink = $"{dto.VerificationUrl}?token={token}&userId={userId}";
            var body = $"<p>Vui lòng nhấp vào liên kết sau để xác thực email khôi phục:</p><a href='{verificationLink}'>Xác thực Email</a>";
            await _emailService.SendEmailAsync(dto.RecoveryEmail, "Xác thực email khôi phục", body);
        }

        public async Task<bool> VerifyRecoveryEmailTokenAsync(long userId, string token)
        {
            var user = await _userRepository.GetUserByUserIdAsync(userId);
            if (user == null || user.PasswordResetToken != token || user.ResetTokenExpiry <= DateTime.UtcNow)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(user.PendingRecoveryEmail))
            {
                user.RecoveryEmail = user.PendingRecoveryEmail; 
                user.PendingRecoveryEmail = null;               
                user.IsRecoveryEmailVerified = true;           
            }

            user.PasswordResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        private string GenerateSecureToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
                .Replace('+', '-')
                .Replace('/', '_');
        }

        public async Task<List<RecentUserDTO>> GetRecentUsersAsync()
        {
            var recentUsers = await _userRepository.GetRecentUsersAsync(5);

            // Mapping từ User (Entity) sang RecentUserDTO (API)
            return recentUsers.Select(u => new RecentUserDTO
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                Email = u.Email,
                CreatedAt = u.CreatedAt
            }).ToList();
        }
    }
}