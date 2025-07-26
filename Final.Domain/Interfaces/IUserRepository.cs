using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Queries;

namespace Final.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> CreateUser(User user);
        Task<User?> GetUserByUserIdAsync(long userId);
        Task UpdateUserAsync(User user);
        Task<PagedResult<User>> GetAllUserAsync(UserQuery query);

        Task<int> CountUsersInDateRange(DateTime startDate, DateTime endDate);
        Task<List<User>> GetRecentUsersAsync(int count);
    }
}
