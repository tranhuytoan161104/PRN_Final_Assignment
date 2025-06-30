using Final.Domain.Enums;   
using System;
using System.Collections.Generic;

namespace Final.Domain.Entities
{
    public class Order
    {
        public long Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public EOrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public long UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}