using Final.Domain.Enums;

namespace Final.OrderAPI.DTOs
{
    public class PaymentTransactionDTO
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public EPaymentStatus Status { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string TransactionId { get; set; } = null!;
        public long OrderId { get; set; }
    }
}
