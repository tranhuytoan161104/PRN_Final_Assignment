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

        public async Task<User?> GetUserByIdAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<User>> GetAllUserAsync(UserQuery query)
        {
            var usersQuery = _context.Users.AsQueryable();

            // Logic lọc và sắp xếp có thể thêm ở đây sau này

            usersQuery = usersQuery.OrderBy(u => u.Id);

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
    }
}
