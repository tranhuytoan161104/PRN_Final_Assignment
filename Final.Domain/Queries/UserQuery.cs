using Final.Domain.Enums;

namespace Final.Domain.Queries
{
    public class UserQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? Role { get; set; }
        public EUserStatus? Status { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";
    }
}