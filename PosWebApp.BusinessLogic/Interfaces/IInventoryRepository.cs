using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Product>> GetAllItemsAsync();
        Task<Product?> GetItemByIdAsync(int itemId);
        Task<Product?> GetItemByNameAsync(string itemName);
        Task<int> AddItemAsync(Product product);
        Task<int> UpdateItemAsync(Product product);
        Task<int> DeleteItemAsync(int itemId);
        Task<int> DecrementItemQuantityAsync(int itemId, int quantity);
    }
}
