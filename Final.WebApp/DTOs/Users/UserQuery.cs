namespace Final.WebApp.DTOs.Users
{
    public class UserQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; } 
    }
}
