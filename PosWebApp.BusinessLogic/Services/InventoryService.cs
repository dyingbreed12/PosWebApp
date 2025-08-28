// In a new file, InventoryService.cs

using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Models;
using System.Security.Cryptography;
using System.Text;

namespace PosWebAppBusinessLogic.Services
{
    public class InventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // Method to generate a unique SKU
        public string GenerateSku(string itemName)
        {
            // Use the first 3 letters of the item name and a unique hash
            var prefix = itemName.Length >= 3 ? itemName.Substring(0, 3).ToUpper() : itemName.ToUpper();
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Create a random, unique part of the SKU
                var uniquePart = Guid.NewGuid().ToString().Replace("-", "");
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(uniquePart));
                var hash = BitConverter.ToString(bytes).Replace("-", "").Substring(0, 5);
                return $"{prefix}-{hash}";
            }
        }

        // This method will now handle all CRUD logic
        public async Task<bool> AddItem(Item item)
        {
            if (string.IsNullOrEmpty(item.SKU))
            {
                item.SKU = GenerateSku(item.ItemName);
            }
            return await _inventoryRepository.AddAsync(item) > 0;
        }

        // Other methods can be added here
    }
}