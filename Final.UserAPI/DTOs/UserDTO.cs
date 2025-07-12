using Final.Domain.Entities;
using Final.Domain.Enums;

namespace Final.UserAPI.DTOs
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public EUserStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
