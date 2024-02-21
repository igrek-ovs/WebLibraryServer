using System.ComponentModel.DataAnnotations.Schema;

namespace testWabApi1.Models
{
    public class PaymentTransaction
    {
        public int Id { get; set; }
        public string PaymentIntentId { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
