namespace PosWebAppCommon
{
    public class DailySalesDto
    {
        public DateTime SalesDate { get; set; }
        public decimal TotalSales { get; set; } // Renamed from TotalAmount for clarity
    }
}
