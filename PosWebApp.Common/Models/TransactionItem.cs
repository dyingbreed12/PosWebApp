namespace PosWebAppCommon.Models
{
    public class TransactionItem
    {
        public int TransactionItemId { get; set; }
        public int TransactionId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
        public int UserId { get; set; }
    }
}
