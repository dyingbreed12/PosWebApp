using PosWebAppCommon;

namespace PosWebApp.Models
{
    public class DashboardViewModel
    {
        public decimal TotalSales { get; set; }
        public int TotalTransactions { get; set; }
        public IEnumerable<DailySalesDto> DailySales { get; set; }
    }
}
