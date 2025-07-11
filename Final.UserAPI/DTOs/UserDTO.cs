using Final.Domain.Entities;

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
        public DateTime CreatedAt { get; set; }
    }
}
