using System.Collections.Generic;
using System;

namespace Final.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}