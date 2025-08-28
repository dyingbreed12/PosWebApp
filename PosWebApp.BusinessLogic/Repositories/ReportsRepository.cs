using Dapper;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.DTOs;
using PosWebAppCommon.Interfaces;

namespace PosWebAppBusinessLogic.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly IDapperContext _context;

        public ReportsRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesHistoryReportDto>> GetSalesHistory(DateTime? startDate, DateTime? endDate)
        {
            var sql = @"
            SELECT
                ST.TransactionDate,
                ST.TotalAmount,
                U.Username AS Cashier,
                TI.ItemName,
                TI.Quantity,
                TI.PricePerItem AS ItemPrice
            FROM SalesTransactions ST
            JOIN Users U ON ST.UserId = U.Id
            JOIN TransactionItems TI ON ST.TransactionId = TI.TransactionId
            WHERE (@StartDate IS NULL OR ST.TransactionDate >= @StartDate)
              AND (@EndDate IS NULL OR ST.TransactionDate <= @EndDate)
            ORDER BY ST.TransactionDate DESC";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<SalesHistoryReportDto>(sql, new { StartDate = startDate, EndDate = endDate });
            }
        }

        public async Task<IEnumerable<InventoryReportDto>> GetInventoryLevels()
        {
            var sql = @"
            SELECT
                ItemName,
                SKU,
                Quantity,
                ReorderLevel,
                SellingPrice,
                CostPrice,
                IsActive
            FROM Inventory
            ORDER BY ItemName";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<InventoryReportDto>(sql);
            }
        }

        public async Task<IEnumerable<ProfitLossReportDto>> GetProfitLoss(DateTime? startDate, DateTime? endDate)
        {
            var sql = @"
            SELECT
                CAST(ST.TransactionDate AS DATE) AS Date,
                SUM(TI.Quantity * I.SellingPrice) AS TotalSales,
                SUM(TI.Quantity * I.CostPrice) AS TotalCost,
                SUM(TI.Quantity * (I.SellingPrice - I.CostPrice)) AS TotalProfit
            FROM SalesTransactions ST
            JOIN TransactionItems TI ON ST.TransactionId = TI.TransactionId
            JOIN Inventory I ON TI.ItemId = I.ItemId
            WHERE (@StartDate IS NULL OR ST.TransactionDate >= @StartDate)
              AND (@EndDate IS NULL OR ST.TransactionDate <= @EndDate)
            GROUP BY CAST(ST.TransactionDate AS DATE)
            ORDER BY Date DESC";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<ProfitLossReportDto>(sql, new { StartDate = startDate, EndDate = endDate });
            }
        }
    }
}