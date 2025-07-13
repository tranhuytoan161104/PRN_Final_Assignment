using Final.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Domain.Entities
{
    public class PaymentTransaction
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public EPaymentStatus Status { get; set; }
        public string PaymentMethod { get; set; }
        public string? TransactionId { get; set; }

        public long OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
