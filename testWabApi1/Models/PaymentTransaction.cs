namespace testWabApi1.Models
{
    public class PaymentTransaction
    {
        public int Id { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
