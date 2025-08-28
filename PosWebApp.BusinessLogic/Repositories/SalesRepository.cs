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
                // This will execute the query and return the new Id
                return await connection.ExecuteScalarAsync<int>(sql, new { TotalAmount = totalAmount, UserId = userId });
            }
        }

        public async Task AddTransactionItems(IEnumerable<TransactionItem> items)
        {
            // The SQL query should insert into the TransactionItems table, not SalesTransactions
            var sql = @"
                INSERT INTO TransactionItems (TransactionId, ItemId, ItemName, Quantity, PricePerItem)
                VALUES (@TransactionId, @ItemId, @ItemName, @Quantity, @PricePerItem);";

            using (var connection = _context.CreateConnection())
            {
                // Dapper's ExecuteAsync method is perfect for inserting a list of items
                await connection.ExecuteAsync(sql, items);
            }
        }
    }
}