using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Product?> GetProductById(int itemId);
        Task<Product?> GetProductByName(string itemName);
        Task DecrementProductQuantity(int itemId, int quantity);
    }
}
