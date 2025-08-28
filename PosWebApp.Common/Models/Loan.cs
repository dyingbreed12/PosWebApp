namespace PosWebAppCommon.Models
{
    public class Loan : BaseEntity
    {
        public int? CustomerId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; } // 'Active', 'Paid', 'Overdue'
        public string? Notes { get; set; }
    }
}
