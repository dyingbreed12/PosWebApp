using Dapper;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Interfaces;
using PosWebAppCommon.Models;
using System.Data;

namespace PosWebAppBusinessLogic.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IDapperContext _context;

        public InventoryRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllItemsAsync()
        {
            var sql = "SELECT * FROM Inventory ORDER BY ItemName";
            using (IDbConnection connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Product>(sql);
            }
        }

        public async Task<Product?> GetItemByIdAsync(int itemId)
        {
            var sql = "SELECT * FROM Inventory WHERE ItemId = @ItemId";
            using (IDbConnection connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { ItemId = itemId });
            }
        }

        public async Task<Product?> GetItemByNameAsync(string itemName)
        {
            var sql = "SELECT * FROM Inventory WHERE ItemName = @ItemName";
            using (IDbConnection connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { ItemName = itemName });
            }
        }

        public async Task<int> AddItemAsync(Product product)
        {
            var sql = "INSERT INTO Inventory (ItemName, Quantity, Price) VALUES (@ItemName, @Quantity, @Price)";
            using (IDbConnection connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(sql, new { product.ItemName, product.Quantity, product.Price });
            }
        }

        public async Task<int> UpdateItemAsync(Product product)
        {
            var sql = "UPDATE Inventory SET ItemName = @ItemName, Quantity = @Quantity, Price = @Price WHERE ItemId = @ItemId";
            using (IDbConnection connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(sql, product);
            }
        }

        public async Task<int> DeleteItemAsync(int itemId)
        {
            var sql = "DELETE FROM Inventory WHERE ItemId = @ItemId";
            using (IDbConnection connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(sql, new { ItemId = itemId });
            }
        }

        public async Task<int> DecrementItemQuantityAsync(int itemId, int quantity)
        {
            var sql = "UPDATE Inventory SET Quantity = Quantity - @quantity WHERE ItemId = @ItemId AND Quantity >= @quantity";
            using (IDbConnection connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(sql, new { itemId, quantity });
            }
        }
    }
}