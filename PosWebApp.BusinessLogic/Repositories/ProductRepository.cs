using Dapper;
using PosWebAppCommon.Interfaces;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDapperContext _context; // Use the interface here

        public ProductRepository(IDapperContext context) // The constructor is now generic
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var sql = "SELECT ItemId, ItemName, Quantity, Price, CreatedAt FROM Inventory";
            using (var connection = _context.CreateConnection())
            {
                var products = await connection.QueryAsync<Product>(sql);

                return products.ToList();
            }
        }

        public async Task<Product?> GetProductById(int itemId)
        {
            var sql = "SELECT ItemId, ItemName, Quantity, Price, CreatedAt FROM Inventory WHERE ItemId = @ItemId";
            using (var connection = _context.CreateConnection())
            {
                var product = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { ItemId = itemId });

                return product;
            }
        }

        public async Task<Product?> GetProductByName(string itemName)
        {
            var sql = "SELECT ItemId, ItemName, Quantity, Price, CreatedAt FROM Inventory WHERE ItemName = @ItemName";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Product>(sql, new { ItemName = itemName });
            }
        }

        // This is a critical method for the POS counter
        public async Task DecrementProductQuantity(int itemId, int quantity)
        {
            var sql = "UPDATE Inventory SET Quantity = Quantity - @Quantity WHERE ItemId = @ItemId AND Quantity >= @Quantity";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { ItemId = itemId, Quantity = quantity });
            }
        }
    }
}
