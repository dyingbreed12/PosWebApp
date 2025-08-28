namespace PosWebAppCommon.Models
{
    public class SalesTransaction
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
    }
}
