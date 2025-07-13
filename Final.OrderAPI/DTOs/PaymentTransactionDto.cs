using Final.Domain.Enums;

namespace Final.OrderAPI.DTOs
{
    public class PaymentTransactionDto
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public EPaymentStatus Status { get; set; }
        public string PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public string? FailureReason { get; set; }
        public long OrderId { get; set; }
    }
}
