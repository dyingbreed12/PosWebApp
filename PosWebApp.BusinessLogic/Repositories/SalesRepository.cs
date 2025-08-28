using Dapper;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Interfaces;
using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private readonly IDapperContext _context;

        public SalesRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateSalesTransaction(decimal totalAmount, int userId)
        {
            var sql = "INSERT INTO SalesTransactions (TotalAmount, UserId) VALUES (@TotalAmount, @UserId); SELECT SCOPE_IDENTITY();";
            using (var connection = _context.CreateConnection())
            {
                var transactionId = await connection.QueryAsync<int>(sql, new { TotalAmount = totalAmount, UserId = userId });

                return transactionId.Single();
            }
        }

        public async Task AddTransactionItems(IEnumerable<TransactionItem> items)
        {
            var sql = "INSERT INTO SalesTransactions (TotalAmount, UserId) VALUES (@Quantity, @UserId); SELECT SCOPE_IDENTITY();";
            using (var connection = _context.CreateConnection())
            {
                var transactionId = await connection.ExecuteAsync(sql, items);
            }
        }
    }
}
