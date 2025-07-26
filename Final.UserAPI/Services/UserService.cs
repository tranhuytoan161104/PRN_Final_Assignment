using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Enums;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
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

        /// <summary>
        /// Đăng ký một người dùng mới vào hệ thống.
        /// </summary>
        /// <param name="registerDto">Thông tin đăng ký của người dùng.</param>
        /// <returns>Thông tin chi tiết của người dùng vừa được tạo.</returns>
        /// <exception cref="InvalidOperationException">Ném ngoại lệ nếu email đã tồn tại.</exception>
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

        /// <summary>
        /// Xác thực thông tin đăng nhập và tạo token cho người dùng.
        /// </summary>
        /// <param name="loginDto">Thông tin đăng nhập (email và mật khẩu).</param>
        /// <returns>Một đối tượng chứa Access Token.</returns>
        /// <exception cref="UnauthorizedAccessException">Ném ngoại lệ nếu thông tin đăng nhập không hợp lệ hoặc tài khoản bị khóa.</exception>
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

        /// <summary>
        /// Lấy thông tin hồ sơ của một người dùng dựa trên ID.
        /// </summary>
        /// <param name="userId">ID của người dùng cần lấy thông tin.</param>
        /// <returns>Thông tin hồ sơ của người dùng.</returns>
        /// <exception cref="KeyNotFoundException">Ném ngoại lệ nếu không tìm thấy người dùng.</exception>
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

        /// <summary>
        /// Cập nhật thông tin hồ sơ (tên, họ) của người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng cần cập nhật.</param>
        /// <param name="updateDto">Thông tin cần cập nhật.</param>
        /// <returns>Thông tin hồ sơ sau khi đã cập nhật.</returns>
        /// <exception cref="KeyNotFoundException">Ném ngoại lệ nếu không tìm thấy người dùng.</exception>
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

        /// <summary>
        /// Thay đổi mật khẩu cho người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng cần đổi mật khẩu.</param>
        /// <param name="changePasswordDto">Chứa mật khẩu cũ và mật khẩu mới.</param>
        /// <returns>Trả về true nếu đổi mật khẩu thành công.</returns>
        /// <exception cref="KeyNotFoundException">Ném ngoại lệ nếu không tìm thấy người dùng.</exception>
        /// <exception cref="InvalidOperationException">Ném ngoại lệ nếu mật khẩu cũ không chính xác.</exception>
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

        /// <summary>
        /// Lấy danh sách tất cả người dùng với phân trang (chỉ dành cho Admin).
        /// </summary>
        /// <param name="query">Thông tin phân trang.</param>
        /// <returns>Danh sách người dùng đã được phân trang.</returns>
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

        /// <summary>
        /// Cập nhật vai trò của một người dùng (chỉ dành cho Owner).
        /// </summary>
        /// <param name="userId">ID của người dùng cần cập nhật.</param>
        /// <param name="userRoleDto">Vai trò mới của người dùng.</param>
        /// <returns>Thông tin người dùng sau khi cập nhật.</returns>
        /// <exception cref="KeyNotFoundException">Ném ngoại lệ nếu không tìm thấy người dùng.</exception>
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

        /// <summary>
        /// Cập nhật trạng thái (active/inactive) của một người dùng (chỉ dành cho Admin).
        /// </summary>
        /// <param name="userId">ID của người dùng cần cập nhật.</param>
        /// <param name="updateUserStatusDto">Trạng thái mới của người dùng.</param>
        /// <returns>Thông tin người dùng sau khi cập nhật.</returns>
        /// <exception cref="KeyNotFoundException">Ném ngoại lệ nếu không tìm thấy người dùng.</exception>
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
            user.RecoveryEmail = dto.RecoveryEmail;
            user.IsRecoveryEmailVerified = false;
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
            user.IsRecoveryEmailVerified = true;
            // Giữ lại RecoveryEmail, chỉ xóa token
            user.PasswordResetToken = null;
            user.ResetTokenExpiry = null;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        // (Các phương thức private và helper khác như MapToUserDTO, GenerateSecureToken...)
        private string GenerateSecureToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}