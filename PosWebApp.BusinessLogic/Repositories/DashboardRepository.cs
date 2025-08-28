using Dapper;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.DTOs;
using PosWebAppCommon.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PosWebAppBusinessLogic.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IDapperContext _context;

        public DashboardRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalSalesForPeriod(DateTime startDate, DateTime endDate)
        {
            var sql = "SELECT ISNULL(SUM(TotalAmount), 0) FROM SalesTransactions WHERE CreatedAt BETWEEN @StartDate AND @EndDate";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<decimal>(sql, new { StartDate = startDate, EndDate = endDate });
            }
        }

        public async Task<decimal> GetTotalProfitForPeriod(DateTime startDate, DateTime endDate)
        {
            var sql = @"
                SELECT ISNULL(SUM(TI.Quantity * (I.SellingPrice - I.CostPrice)), 0)
                FROM TransactionItems TI
                JOIN SalesTransactions ST ON TI.TransactionId = ST.Id
                JOIN Inventory I ON TI.ItemId = I.Id
                WHERE ST.CreatedAt BETWEEN @StartDate AND @EndDate";

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<decimal>(sql, new { StartDate = startDate, EndDate = endDate });
            }
        }

        public async Task<int> GetTotalTransactionsCount()
        {
            var sql = "SELECT COUNT(Id) FROM SalesTransactions";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task<int> GetLowStockItemsCount(int reorderLevel)
        {
            var sql = "SELECT COUNT(Id) FROM Inventory WHERE Quantity <= ReorderLevel";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task<IEnumerable<DailySalesDto>> GetDailySalesForPeriod(DateTime startDate, DateTime endDate)
        {
            var sql = @"
                SELECT
                    CAST(CreatedAt AS DATE) as SalesDate,
                    SUM(TotalAmount) as TotalSales
                FROM SalesTransactions
                WHERE CreatedAt BETWEEN @StartDate AND @EndDate
                GROUP BY CAST(CreatedAt AS DATE)
                ORDER BY SalesDate ASC";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<DailySalesDto>(sql, new { StartDate = startDate, EndDate = endDate });
            }
        }

        public async Task<IEnumerable<TopSellingItemDto>> GetTopSellingItems(int count)
        {
            var sql = @"
                SELECT TOP (@Count)
                    I.ItemName,
                    SUM(TI.Quantity) as TotalSold
                FROM TransactionItems TI
                JOIN Inventory I ON TI.ItemId = I.Id
                GROUP BY I.ItemName
                ORDER BY TotalSold DESC";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<TopSellingItemDto>(sql, new { Count = count });
            }
        }
    }
}