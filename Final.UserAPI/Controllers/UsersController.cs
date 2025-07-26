using Final.Domain.Common;
using Final.Domain.Queries;
using Final.UserAPI.DTOs;
using Final.UserAPI.DTOs.PasswordReset;
using Final.UserAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Final.UserAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private long CurrentUserId
        {
            get
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out var userId))
                {
                    throw new UnauthorizedAccessException("Token không hợp lệ hoặc không chứa ID người dùng.");
                }
                return userId;
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterDTO registerDto)
        {
            var user = await _userService.RegisterUserAsync(registerDto);
            return CreatedAtRoute("GetUserProfileByUserIdAsync", new { userId = user.Id }, user);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenDTO>> LoginUserAsync([FromBody] LoginDTO loginDto)
        {
            var tokenDto = await _userService.LoginUserAsync(loginDto);
            return Ok(tokenDto);
        }

        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDTO>> GetCurrentUserProfileAsync()
        {
            var userProfile = await _userService.GetUserProfileByUserIdAsync(CurrentUserId);
            return Ok(userProfile);
        }

        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDTO>> UpdateCurrentUserProfileAsync([FromBody] UpdateProfileDTO updateDto)
        {
            var updatedProfile = await _userService.UpdateUserProfileByUserIdAsync(CurrentUserId, updateDto);
            return Ok(updatedProfile);
        }

        [HttpPatch("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangeCurrentUserPasswordAsync([FromBody] ChangePasswordDTO changePasswordDto)
        {
            await _userService.ChangeUserPasswordByUserIdAsync(CurrentUserId, changePasswordDto);
            return Ok(new { message = "Đổi mật khẩu thành công." });
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType(typeof(PagedResult<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PagedResult<UserDTO>>> GetAllUsersAsync([FromQuery] UserQuery queries)
        {
            var pagedResult = await _userService.GetAllUsersAsync(queries);
            return Ok(pagedResult);
        }

        [HttpGet("{userId:long}", Name = "GetUserProfileByUserIdAsync")]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType(typeof(UserProfileDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfileByUserIdAsync(long userId)
        {
            var userProfile = await _userService.GetUserProfileByUserIdAsync(userId);
            return Ok(userProfile);
        }

        [HttpPatch("{userId:long}/role")]
        [Authorize(Roles = "Owner")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDTO>> UpdateUserRoleByUserIdAsync(long userId, [FromBody] UserRoleDTO userRoleDto)
        {
            var updatedUser = await _userService.UpdateUserRoleByUserIdAsync(userId, userRoleDto);
            return Ok(updatedUser);
        }

        [HttpPatch("{userId:long}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDTO>> UpdateUserStatusByUserIdAsync(long userId, [FromBody] UpdateUserStatusDTO updateUserStatusDto)
        {
            var updatedUser = await _userService.UpdateUserStatusByUserIdAsync(userId, updateUserStatusDto);
            return Ok(updatedUser);
        }

        [HttpPost("setup-security-question")]
        [AllowAnonymous]
        public async Task<IActionResult> SetupSecurityQuestion([FromBody] SetupSecurityQuestionDTO dto)
        {
            await _userService.SetupSecurityQuestionAsync(dto);
            return Ok(new { message = "Thiết lập câu hỏi bảo mật thành công." });
        }

        [HttpPost("forgot-password/question")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSecurityQuestion([FromBody] ForgotPasswordRequestDTO dto)
        {
            var question = await _userService.GetSecurityQuestionByEmailAsync(dto.Email);
            return Ok(new { question });
        }

        [HttpPost("forgot-password/verify-answer")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAnswer([FromBody] VerifySecurityAnswerDTO dto)
        {
            var token = await _userService.VerifySecurityAnswerAndGenerateTokenAsync(dto);
            return Ok(new { resetToken = token });
        }

        [HttpPost("forgot-password/send-email")]
        [AllowAnonymous]
        public async Task<IActionResult> SendRecoveryEmail([FromBody] SendRecoveryEmailDTO dto)
        {
            await _userService.SendRecoveryEmailAsync(dto);
            return Ok(new { message = "Nếu email khôi phục của bạn tồn tại và đã được xác thực, chúng tôi đã gửi một liên kết đặt lại mật khẩu." });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            await _userService.ResetPasswordAsync(dto);
            return Ok(new { message = "Đặt lại mật khẩu thành công." });
        }

        [HttpPost("profile/link-recovery-email")]
        [Authorize]
        public async Task<IActionResult> LinkRecoveryEmail([FromBody] LinkRecoveryEmailDTO dto)
        {
            await _userService.SendVerificationEmailAsync(CurrentUserId, dto);
            return Ok(new { message = "Email xác thực đã được gửi đến địa chỉ mới." });
        }

        [HttpGet("verify-recovery-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyRecoveryEmail([FromQuery] long userId, [FromQuery] string token)
        {
            var isSuccess = await _userService.VerifyRecoveryEmailTokenAsync(userId, token);
            if (isSuccess)
            {
                return Content("<h1>Xác thực email thành công!</h1><p>Bạn có thể đóng cửa sổ này.</p>", "text/html; charset=utf-8");
            }
            return Content("<h1>Xác thực thất bại.</h1><p>Liên kết không hợp lệ hoặc đã hết hạn.</p>", "text/html; charset=utf-8");
        }

        [HttpGet("recent")]
        [Authorize(Roles = "Admin, Owner")]
        public async Task<IActionResult> GetRecentUsers()
        {
            var recentUsers = await _userService.GetRecentUsersAsync();
            return Ok(recentUsers);
        }
    }
}