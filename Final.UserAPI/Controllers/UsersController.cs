using Final.Domain.Common;
using Final.Domain.Queries;
using Final.UserAPI.DTOs;
using Final.UserAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Final.UserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">Ném ngoại lệ nếu không tìm thấy thông tin người dùng trong token.</exception>
        private long CurrentUserId
        {
            get
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out var userId))
                {
                    // Lỗi này không nên xảy ra nếu [Authorize] hoạt động đúng, nhưng vẫn là một lớp phòng thủ tốt.
                    throw new UnauthorizedAccessException("Token không hợp lệ hoặc không chứa ID người dùng.");
                }
                return userId;
            }
        }

        /// <summary>
        /// Đăng ký một tài khoản người dùng mới.
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
            // Sử dụng CreatedAtRoute để trả về URL của endpoint lấy thông tin người dùng theo ID
            return CreatedAtRoute("GetUserProfileById", new { userId = user.Id }, user);
        }

        /// <summary>
        /// Đăng nhập vào hệ thống.
        /// </summary>
        /// <param name="loginDto">Thông tin đăng nhập (email và mật khẩu).</param>
        /// <returns>Một token JWT để xác thực các yêu cầu sau này.</returns>
        /// <response code="200">Đăng nhập thành công và trả về token.</response>
        /// <response code="401">Nếu thông tin đăng nhập không chính xác hoặc tài khoản bị khóa.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenDTO>> LoginAsync([FromBody] LoginDTO loginDto)
        {
            var tokenDto = await _userService.LoginUserAsync(loginDto);
            return Ok(tokenDto);
        }

        /// <summary>
        /// Lấy thông tin hồ sơ của người dùng hiện tại (đã đăng nhập).
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
        public async Task<ActionResult<UserProfileDTO>> GetUserProfileAsync()
        {
            var userProfile = await _userService.GetUserProfileAsync(CurrentUserId);
            return Ok(userProfile);
        }

        /// <summary>
        /// Cập nhật thông tin hồ sơ (tên, họ) của người dùng hiện tại.
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
        public async Task<ActionResult<UserProfileDTO>> UpdateUserProfileAsync([FromBody] UpdateProfileDTO updateDto)
        {
            var updatedProfile = await _userService.UpdateUserProfileAsync(CurrentUserId, updateDto);
            return Ok(updatedProfile);
        }

        /// <summary>
        /// Thay đổi mật khẩu của người dùng hiện tại.
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
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDTO changePasswordDto)
        {
            await _userService.ChangeUserPasswordAsync(CurrentUserId, changePasswordDto);
            return Ok(new { message = "Đổi mật khẩu thành công." });
        }

        /// <summary>
        /// [Admin] Lấy danh sách tất cả người dùng với phân trang.
        /// </summary>
        /// <param name="query">Các tham số truy vấn và phân trang.</param>
        /// <returns>Danh sách người dùng đã được phân trang.</returns>
        /// <response code="200">Trả về danh sách người dùng.</response>
        /// <response code="401">Nếu người dùng chưa đăng nhập.</response>
        /// <response code="403">Nếu người dùng không có quyền Admin.</response>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PagedResult<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PagedResult<UserDTO>>> GetAllUsersAsync([FromQuery] UserQuery query)
        {
            var pagedResult = await _userService.GetAllUsersAsync(query);
            return Ok(pagedResult);
        }

        /// <summary>
        /// [Admin] Lấy thông tin hồ sơ của một người dùng bất kỳ bằng ID.
        /// </summary>
        /// <param name="userId">ID của người dùng cần lấy thông tin.</param>
        /// <returns>Thông tin hồ sơ của người dùng.</returns>
        /// <response code="200">Trả về thông tin hồ sơ.</response>
        /// <response code="404">Nếu không tìm thấy người dùng.</response>
        [HttpGet("{userId:long}", Name = "GetUserProfileById")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserProfileDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfileByIdAsync(long userId)
        {
            var userProfile = await _userService.GetUserProfileAsync(userId);
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
        public async Task<ActionResult<UserDTO>> UpdateUserRoleAsync(long userId, [FromBody] UserRoleDTO userRoleDto)
        {
            var updatedUser = await _userService.UpdateUserRoleAsync(userId, userRoleDto);
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
        public async Task<ActionResult<UserDTO>> UpdateUserStatusAsync(long userId, [FromBody] UpdateUserStatusDTO updateUserStatusDto)
        {
            var updatedUser = await _userService.UpdateUserStatusAsync(userId, updateUserStatusDto);
            return Ok(updatedUser);
        }
    }
}
