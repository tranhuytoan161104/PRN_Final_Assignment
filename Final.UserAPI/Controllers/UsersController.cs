using Final.Domain.Entities;
using Final.Domain.Queries;
using Final.UserAPI.DTOs;
using Final.UserAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Final.UserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var user = await _userService.RegisterUser(registerDto);
                return CreatedAtAction(nameof(RegisterUser), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var tokenDto = await _userService.LoginUser(loginDto);
                return Ok(tokenDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdString) || !long.TryParse(userIdString, out var userId))
                {
                    return Unauthorized("Token invalid or do not contain user's id.");
                }

                var userProfile = await _userService.GetUserProfileAsync(userId);

                if (userProfile == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateProfileDTO updateDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Invalid Token.");
            }

            try
            {
                var updatedProfile = await _userService.UpdateUserProfileAsync(userId, updateDto);

                if (updatedProfile == null)
                {
                    return NotFound("User do not exists.");
                }

                return Ok(updatedProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPatch("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Invalid Token.");
            }

            try
            {
                var result = await _userService.ChangeUserPasswordAsync(userId, changePasswordDto);
                if (!result)
                {
                    return NotFound("User do not exists.");
                }

                return Ok(new { message = "Password change successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] UserQuery query)
        {
            var pagedResult = await _userService.GetAllUsersAsync(query);
            return Ok(pagedResult);
        }

        [HttpPatch("{userId}/role")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> UpdateUserRole(long userId, [FromBody] UserRoleDTO userRoleDto)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserRoleAsync(userId, userRoleDto);
                if (updatedUser == null)
                {
                    return NotFound($"User with Id {userId} not found");
                }
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPatch("{userId}/status")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> UpdateUserStatus(long userId, [FromBody] UpdateUserStatusDTO updateUserStatusDto)
        {
            var updatedUser = await _userService.UpdateUserStatusAsync(userId, updateUserStatusDto);
            if (updatedUser == null)
            {
                return NotFound($"Không tìm thấy người dùng với ID: {userId}");
            }
            return Ok(updatedUser);
        }
    }
}
