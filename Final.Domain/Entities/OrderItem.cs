using System.ComponentModel.DataAnnotations.Schema;

namespace Final.Domain.Entities
{
    public class OrderItem
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}