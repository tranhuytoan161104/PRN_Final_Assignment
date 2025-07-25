using System.Collections.Generic;

namespace Final.Domain.Entities
{
    public class ShoppingCart
    {
        public long Id { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public virtual ICollection<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    }
}