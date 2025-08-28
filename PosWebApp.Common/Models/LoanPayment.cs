namespace PosWebAppCommon.Models
{
    public class LoanPayment : BaseEntity
    {
        public int LoanId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}
