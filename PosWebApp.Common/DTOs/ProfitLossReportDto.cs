namespace PosWebAppCommon.DTOs
{
    public class ProfitLossReportDto
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
    }
}
