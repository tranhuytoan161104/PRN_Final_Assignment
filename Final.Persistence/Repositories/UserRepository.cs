using Final.Domain.Common;
using Final.Domain.Entities;
using Final.Domain.Interfaces;
using Final.Domain.Queries;
using Final.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByUserIdAsync(long userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<User>> GetAllUserAsync(UserQuery query)
        {
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var searchTermLower = query.SearchTerm.ToLower().Trim();
                usersQuery = usersQuery.Where(u =>
                    (u.FirstName + " " + u.LastName).ToLower().Contains(searchTermLower) ||
                    u.Email.ToLower().Contains(searchTermLower));
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                usersQuery = usersQuery.Where(u => u.Role == query.Role);
            }

            if (query.Status.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.Status == query.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                bool isDescending = query.SortDirection?.ToLower() == "desc";
                switch (query.SortBy.ToLower())
                {
                    case "email":
                        usersQuery = isDescending ? usersQuery.OrderByDescending(u => u.Email) : usersQuery.OrderBy(u => u.Email);
                        break;
                    case "name":
                        usersQuery = isDescending ? usersQuery.OrderByDescending(u => u.FirstName).ThenByDescending(u => u.LastName) : usersQuery.OrderBy(u => u.FirstName).ThenBy(u => u.LastName);
                        break;
                    case "createdat":
                        usersQuery = isDescending ? usersQuery.OrderByDescending(u => u.CreatedAt) : usersQuery.OrderBy(u => u.CreatedAt);
                        break;
                    default:
                        usersQuery = usersQuery.OrderBy(u => u.Id);
                        break;
                }
            }
            else
            {
                usersQuery = usersQuery.OrderBy(u => u.Id);
            }

            var totalItems = await usersQuery.CountAsync();
            var items = await usersQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize)
            };
        }

        public async Task<int> CountUsersInDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Users
                .CountAsync(u => u.CreatedAt >= startDate && u.CreatedAt < endDate);
        }

        public async Task<List<User>> GetRecentUsersAsync(int count)
        {
            return await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}