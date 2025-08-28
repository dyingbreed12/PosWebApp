using PosWebAppBusinessLogic.DTOs;
using PosWebAppBusinessLogic.Interfaces;

namespace PosWebAppBusinessLogic.Services
{
    public class DashboardService
    {
        private readonly IDashboardRepository _dashboardRepo;

        public DashboardService(IDashboardRepository dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
        }

        public DashboardData GetDashboardData()
        {
            var totalSales = _dashboardRepo.GetTotalSalesAmount();
            var totalTransactions = _dashboardRepo.GetTotalTransactionsCount();
            var dailySales = _dashboardRepo.GetDailySales();

            return new DashboardData
            {
                TotalSales = totalSales,
                TotalTransactions = totalTransactions,
                DailySales = dailySales
            };
        }
    }
}
