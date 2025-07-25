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

        /// <summary>
        /// Lấy thông tin người dùng theo địa chỉ email.
        /// </summary>
        /// <param name="email">Địa chỉ email của người dùng cần tìm.</param>
        /// <returns>Trả về đối tượng người dùng nếu tìm thấy, ngược lại trả về null.</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Tạo một người dùng mới trong cơ sở dữ liệu.
        /// </summary>
        /// <param name="user">Đối tượng người dùng cần tạo.</param>
        /// <returns>Trả về đối tượng người dùng đã được tạo.</returns>
        public async Task<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Lấy thông tin người dùng theo ID người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng cần lấy thông tin.</param>
        /// <returns>Trả về đối tượng người dùng nếu tìm thấy, ngược lại trả về null.</returns>
        public async Task<User?> GetUserByUserIdAsync(long userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        /// <summary>
        /// Cập nhật thông tin người dùng trong cơ sở dữ liệu.
        /// </summary>
        /// <param name="user">Đối tượng người dùng cần cập nhật.</param>
        /// <returns>Trả về một tác vụ bất đồng bộ.</returns>
        public async Task UpdateUserAsync(User user)
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Lấy danh sách người dùng với phân trang và sắp xếp. 
        /// </summary>
        /// <param name="query">Thông tin phân trang và lọc.</param>
        /// <returns>Kết quả phân trang chứa danh sách người dùng.</returns>
        public async Task<PagedResult<User>> GetAllUserAsync(UserQuery query)
        {
            var usersQuery = _context.Users.AsQueryable();

            // 1. Lọc (Filtering)
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

            // 2. Sắp xếp (Sorting)
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

            // 3. Phân trang (Pagination)
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
