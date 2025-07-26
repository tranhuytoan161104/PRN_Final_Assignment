namespace Final.WebApp.DTOs.Users
{
    public class RecentUserDTO
    {
        public long Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
