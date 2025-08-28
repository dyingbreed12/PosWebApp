using PosWebAppCommon.DTOs;

namespace PosWebAppBusinessLogic.Interfaces
{
    public interface IReportsRepository
    {
        Task<IEnumerable<SalesHistoryReportDto>> GetSalesHistory(DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<InventoryReportDto>> GetInventoryLevels();
        Task<IEnumerable<ProfitLossReportDto>> GetProfitLoss(DateTime? startDate, DateTime? endDate);
    }
}
