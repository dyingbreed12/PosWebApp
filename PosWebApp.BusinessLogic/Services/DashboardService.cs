using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon; // Use the DTO from the common library

namespace PosWebAppBusinessLogic.Services
{
    public class DashboardService
    {
        private readonly IDashboardRepository _dashboardRepo;

        public DashboardService(IDashboardRepository dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
        }

        public async Task<DashboardSummaryDto> GetDashboardData()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastSevenDays = today.AddDays(-7);

            // Fetch all data points asynchronously
            var totalSalesTask = _dashboardRepo.GetTotalSalesForPeriod(startOfMonth, today);
            var totalProfitTask = _dashboardRepo.GetTotalProfitForPeriod(startOfMonth, today);
            var totalTransactionsTask = _dashboardRepo.GetTotalTransactionsCount();
            var lowStockItemsTask = _dashboardRepo.GetLowStockItemsCount(10);
            var dailySalesTask = _dashboardRepo.GetDailySalesForPeriod(lastSevenDays, today);
            var topSellingItemsTask = _dashboardRepo.GetTopSellingItems(5);

            // Await all tasks concurrently for better performance
            await Task.WhenAll(
                totalSalesTask,
                totalProfitTask,
                totalTransactionsTask,
                lowStockItemsTask,
                dailySalesTask,
                topSellingItemsTask
            );

            // Map the results to the new DTO
            return new DashboardSummaryDto
            {
                TotalSales = totalSalesTask.Result,
                TotalProfit = totalProfitTask.Result,
                TotalTransactions = totalTransactionsTask.Result,
                LowStockItems = lowStockItemsTask.Result,
                DailySales = dailySalesTask.Result.ToList(),
                TopSellingItems = topSellingItemsTask.Result.ToList()
            };
        }
    }
}