using PosWebAppCommon;

namespace PosWebAppBusinessLogic.DTOs
{
    public class DashboardData
    {
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public IEnumerable<DailySalesDto> DailySales { get; set; }
    }
}
