using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PosWebAppBusinessLogic.Services
{
    public class ReportsService
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportsService(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public async Task<IEnumerable<SalesHistoryReportDto>> GetSalesHistory(DateTime? startDate, DateTime? endDate)
        {
            return await _reportsRepository.GetSalesHistory(startDate, endDate);
        }

        // Method to get inventory levels, calling the repository
        public async Task<IEnumerable<InventoryReportDto>> GetInventoryLevels()
        {
            return await _reportsRepository.GetInventoryLevels();
        }

        // Method to get profit/loss data, calling the repository
        public async Task<IEnumerable<ProfitLossReportDto>> GetProfitLoss(DateTime? startDate, DateTime? endDate)
        {
            return await _reportsRepository.GetProfitLoss(startDate, endDate);
        }
    }
}