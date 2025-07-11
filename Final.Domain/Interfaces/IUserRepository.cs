using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> CreateUser(User user);
        Task<User?> GetUserByIdAsync(long id);
        Task UpdateUserAsync(User user);
        Task<PagedResult<User>> GetAllUserAsync(UserQuery query);
    }
}
