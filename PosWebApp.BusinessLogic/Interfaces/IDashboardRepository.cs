using PosWebAppCommon;

namespace PosWebAppBusinessLogic.Interfaces
{
    public interface IDashboardRepository
    {
        // Change to async
        Task<decimal> GetTotalSalesForPeriod(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalProfitForPeriod(DateTime startDate, DateTime endDate);
        Task<int> GetTotalTransactionsCount();
        Task<int> GetLowStockItemsCount(int reorderLevel);
        Task<IEnumerable<DailySalesDto>> GetDailySalesForPeriod(DateTime startDate, DateTime endDate);
        Task<IEnumerable<TopSellingItemDto>> GetTopSellingItems(int count);
    }
}