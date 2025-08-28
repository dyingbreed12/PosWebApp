using PosWebAppCommon;

namespace PosWebAppBusinessLogic.Interfaces
{
    public interface IDashboardRepository
    {
        decimal GetTotalSalesAmount();
        int GetTotalTransactionsCount();
        IEnumerable<DailySalesDto> GetDailySales();
    }
}
