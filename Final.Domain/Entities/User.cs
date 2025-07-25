using System.Collections.Generic;
using System;
using Final.Domain.Enums;

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
        public EUserStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }

        public virtual ShoppingCart? ShoppingCart { get; set; }
    }
}