namespace PosWebAppCommon.DTOs
{
    public class SalesHistoryReportDto
    {
        public DateTime TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Cashier { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
