namespace PosWebAppCommon.Models
{
    public class SalesTransaction : BaseEntity
    {
        public DateTime TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserId { get; set; }
    }
}
