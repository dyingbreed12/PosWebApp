using Dapper;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon;
using System.Data;

namespace PosWebAppBusinessLogic.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IDbConnection _db;

        public DashboardRepository(IDbConnection db)
        {
            _db = db;
        }

        public decimal GetTotalSalesAmount()
        {
            var sql = "SELECT ISNULL(SUM(TotalAmount), 0) FROM SalesTransactions";
            return _db.ExecuteScalar<decimal>(sql);
        }

        public int GetTotalTransactionsCount()
        {
            var sql = "SELECT COUNT(*) FROM SalesTransactions";
            return _db.ExecuteScalar<int>(sql);
        }

        public IEnumerable<DailySalesDto> GetDailySales()
        {
            var sql = @"
            SELECT
                CAST(TransactionDate AS DATE) as SalesDate,
                SUM(TotalAmount) as TotalAmount
            FROM SalesTransactions
            GROUP BY CAST(TransactionDate AS DATE)
            ORDER BY SalesDate DESC";
            return _db.Query<DailySalesDto>(sql);
        }
    }
}
