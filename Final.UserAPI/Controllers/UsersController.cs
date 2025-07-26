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

        /// <summary>
        /// Lấy ID của người dùng đã được xác thực từ token.
        /// Cho phép truy cập ID người dùng hiện tại trong các phương thức khác.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">Ném ngoại lệ nếu không tìm thấy thông tin người dùng trong token.</exception>
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

        /// <summary>
        /// Đăng ký một tài khoản người dùng mới.
        /// Cho phép người dùng cung cấp thông tin cần thiết để tạo tài khoản mới.
        /// </summary>
        /// <param name="registerDto">Thông tin cần thiết để đăng ký.</param>
        /// <returns>Thông tin của người dùng vừa được tạo.</returns>
        /// <response code="201">Trả về người dùng vừa được tạo.</response>
        /// <response code="400">Nếu thông tin đăng ký không hợp lệ hoặc email đã tồn tại.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterDTO registerDto)
        {
            var user = await _userService.RegisterUserAsync(registerDto);
            return CreatedAtRoute("GetUserProfileByUserIdAsync", new { userId = user.Id }, user);
        }

        /// <summary>
        /// Đăng nhập vào hệ thống.
        /// Cho phép người dùng đăng nhập bằng email và mật khẩu để nhận token xác thực.
        /// </summary>
        /// <param name="loginDto">Thông tin đăng nhập (email và mật khẩu).</param>
        /// <returns>Một token JWT để xác thực các yêu cầu sau này.</returns>
        /// <response code="200">Đăng nhập thành công và trả về token.</response>
        /// <response code="401">Nếu thông tin đăng nhập không chính xác hoặc tài khoản bị khóa.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenDTO>> LoginUserAsync([FromBody] LoginDTO loginDto)
        {
            var tokenDto = await _userService.LoginUserAsync(loginDto);
            return Ok(tokenDto);
        }

        /// <summary>
        /// Lấy thông tin hồ sơ của người dùng hiện tại (đã đăng nhập).
        /// Cho phép người dùng xem thông tin cá nhân của mình như tên, họ, email, v.v.
        /// </summary>
        /// <returns>Thông tin hồ sơ của người dùng.</returns>
        /// <response code="200">Trả về thông tin hồ sơ thành công.</response>
        /// <response code="401">Nếu người dùng chưa đăng nhập.</response>
        /// <response code="404">Nếu không tìm thấy người dùng (ví dụ: đã bị xóa sau khi đăng nhập).</response>
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

        /// <summary>
        /// Cập nhật thông tin hồ sơ (tên, họ) của người dùng hiện tại.
        /// Cho phép người dùng cập nhật thông tin cá nhân của mình.
        /// </summary>
        /// <param name="updateDto">Thông tin cần cập nhật.</param>
        /// <returns>Thông tin hồ sơ sau khi đã cập nhật.</returns>
        /// <response code="200">Cập nhật thành công.</response>
        /// <response code="400">Nếu dữ liệu đầu vào không hợp lệ.</response>
        /// <response code="401">Nếu người dùng chưa đăng nhập.</response>
        /// <response code="404">Nếu không tìm thấy người dùng.</response>
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

        /// <summary>
        /// Thay đổi mật khẩu của người dùng hiện tại.
        /// Cho phép người dùng cập nhật mật khẩu của mình bằng cách cung cấp mật khẩu cũ và mật khẩu mới.
        /// </summary>
        /// <param name="changePasswordDto">Thông tin mật khẩu cũ và mới.</param>
        /// <returns>Một thông báo thành công.</returns>
        /// <response code="200">Đổi mật khẩu thành công.</response>
        /// <response code="400">Nếu mật khẩu cũ không chính xác hoặc mật khẩu mới không hợp lệ.</response>
        /// <response code="401">Nếu người dùng chưa đăng nhập.</response>
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

        /// <summary>
        /// [Admin] Lấy danh sách tất cả người dùng với phân trang.
        /// Cho phép quản trị viên xem danh sách người dùng trong hệ thống với các tham số truy vấn để phân trang và lọc.
        /// </summary>
        /// <param name="queries">Các tham số truy vấn và phân trang.</param>
        /// <returns>Danh sách người dùng đã được phân trang.</returns>
        /// <response code="200">Trả về danh sách người dùng.</response>
        /// <response code="401">Nếu người dùng chưa đăng nhập.</response>
        /// <response code="403">Nếu người dùng không có quyền Admin.</response>
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

        /// <summary>
        /// [Admin] Lấy thông tin hồ sơ của một người dùng bất kỳ bằng ID.
        /// </summary>
        /// <param name="userId">ID của người dùng cần lấy thông tin.</param>
        /// <returns>Thông tin hồ sơ của người dùng.</returns>
        /// <response code="200">Trả về thông tin hồ sơ.</response>
        /// <response code="404">Nếu không tìm thấy người dùng.</response>
        [HttpGet("{userId:long}", Name = "GetUserProfileByUserIdAsync")]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType(typeof(UserProfileDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfileByUserIdAsync(long userId)
        {
            var userProfile = await _userService.GetUserProfileByUserIdAsync(userId);
            return Ok(userProfile);
        }
            
        /// <summary>
        /// [Owner] Cập nhật vai trò của một người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng cần cập nhật.</param>
        /// <param name="userRoleDto">Vai trò mới.</param>
        /// <returns>Thông tin người dùng sau khi cập nhật.</returns>
        /// <response code="200">Cập nhật thành công.</response>
        /// <response code="404">Nếu không tìm thấy người dùng.</response>
        /// <response code="403">Nếu người thực hiện không có quyền Owner.</response>
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

        /// <summary>
        /// [Admin] Cập nhật trạng thái (active/inactive) của một người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng cần cập nhật.</param>
        /// <param name="updateUserStatusDto">Trạng thái mới.</param>
        /// <returns>Thông tin người dùng sau khi cập nhật.</returns>
        /// <response code="200">Cập nhật thành công.</response>
        /// <response code="404">Nếu không tìm thấy người dùng.</response>
        /// <response code="403">Nếu người thực hiện không có quyền Admin.</response>
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

        #region Password Recovery Endpoints
        [HttpPost("setup-security-question")]
        [AllowAnonymous] // SỬA từ [Authorize]
        public async Task<IActionResult> SetupSecurityQuestion([FromBody] SetupSecurityQuestionDTO dto)
        {
            // SỬA logic bên trong
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
        #endregion
    }
}
