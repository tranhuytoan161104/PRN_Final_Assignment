using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Domain.Entities
{
    public class PaymentMethod
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}