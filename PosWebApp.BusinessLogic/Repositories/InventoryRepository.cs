using Dapper;
using PosWebAppBusinessLogic.Common;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Interfaces;
using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Repositories
{
    // The repository now inherits from the generic Dapper repository
    public class InventoryRepository : GenericDapperRepository<Item>, IInventoryRepository
    {
        private readonly IDapperContext _context;

        public InventoryRepository(IDapperContext context) : base(context, "Inventory")
        {
            _context = context;
        }

        public async Task<Item?> GetProductBySku(string sku)
        {
            var sql = "SELECT * FROM Inventory WHERE SKU = @Sku";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Item>(sql, new { Sku = sku });
            }
        }

        public async Task<bool> DecrementProductQuantity(int productId, int quantityToDecrement)
        {
            var sql = @"
                UPDATE Inventory
                SET Quantity = Quantity - @QuantityToDecrement
                WHERE ItemId = @ProductId AND Quantity >= @QuantityToDecrement;";

            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new { ProductId = productId, QuantityToDecrement = quantityToDecrement });
                return rowsAffected > 0;
            }
        }

        public async Task<Item?> GetProductByName(string itemName)
        {
            var sql = "SELECT * FROM Inventory WHERE ItemName = @ItemName";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Item>(sql, new { ItemName = itemName });
            }
        }
    }
}