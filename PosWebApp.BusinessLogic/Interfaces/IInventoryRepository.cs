using PosWebAppBusinessLogic.Common;
using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Interfaces
{
    public interface IInventoryRepository : IGenericDapperRepository<Item>
    {
        Task<Item?> GetProductBySku(string sku);
        Task<bool> DecrementProductQuantity(int productId, int quantityToDecrement);
        Task<Item?> GetProductByName(string itemName);
        Task<IEnumerable<Item>> SearchItems(string query);
    }
}