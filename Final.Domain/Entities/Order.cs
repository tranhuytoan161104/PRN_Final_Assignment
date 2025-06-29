using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Final.Domain.Enums;   

namespace Final.Domain.Entities
{
    public class Order
    {
        [Key]
        public long Id { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

        public EOrderStatus Status { get; set; }

        [Required]
        public string ShippingAddress { get; set; } = null!;

        public long UserId { get; set; }



        [ForeignKey("UserId")]
        public virtual User? User { get; set; }



        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}