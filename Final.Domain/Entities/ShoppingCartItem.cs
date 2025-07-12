namespace Final.Domain.Entities
{
    public class ShoppingCartItem
    {
        public long Id { get; set; }
        public int Quantity { get; set; }

        public long ShoppingCartId { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }

        public long ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}