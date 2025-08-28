using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebApp.Models;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Models;
using System.Security.Claims;
using System.Text.Json;

namespace PosWebApp.Controllers
{
    [Authorize(Roles = "Counter, Admin")] // Only counter and admin can access this
    public class SalesController : Controller
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISalesRepository _salesRepository;

        public SalesController(IInventoryRepository inventoryRepository, ISalesRepository salesRepository)
        {
            _inventoryRepository = inventoryRepository;
            _salesRepository = salesRepository;
        }

        public IActionResult Index()
        {
            // Load the cart from the session
            var cart = GetCartFromSession();
            ViewBag.Cart = cart;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CompleteTransaction()
        {
            var cart = GetCartFromSession();
            if (cart.Any())
            {
                // Get the User ID from claims
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); // We'll need to add UserId to claims later

                // Calculate total amount
                var totalAmount = cart.Sum(item => item.SellingPrice * item.Quantity);

                // Create the sales transaction
                var transactionId = await _salesRepository.CreateSalesTransaction(totalAmount, userId);

                // Prepare and add transaction items
                var transactionItems = cart.Select(item => new TransactionItem
                {
                    TransactionId = transactionId,
                    ItemId = item.Id,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    PricePerItem = item.SellingPrice
                });

                await _salesRepository.AddTransactionItems(transactionItems);

                // Decrement inventory quantity
                foreach (var item in cart)
                {
                   await _inventoryRepository.DecrementProductQuantity(item.Id, item.Quantity);
                }

                HttpContext.Session.Remove("Cart");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string itemName)
        {
            var product = await _inventoryRepository.GetProductByName(itemName);
            if (product != null && product.Quantity > 0)
            {
                var cart = GetCartFromSession();
                // Check if the product is already in the cart
                var cartItem = cart.FirstOrDefault(i => i.Id == product.Id);
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cart.Add(new Item { Id = product.Id, ItemName = product.ItemName, SellingPrice = product.SellingPrice, Quantity = 1 });
                }
                SaveCartToSession(cart);
            }

            return RedirectToAction("Index");
        }

        // Helper methods for session management
        private List<Item>? GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson) ? new List<Item>() : JsonSerializer.Deserialize<List<Item>>(cartJson);
        }

        private void SaveCartToSession(List<Item> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto data)
        {
            var product = await _inventoryRepository.GetProductBySku(data.Sku); // Find by SKU for barcode lookup
            if (product == null || product.Quantity < data.Quantity)
            {
                return Json(new { success = false, message = "Product not found or not enough stock." });
            }

            var cart = GetCartFromSession();
            var cartItem = cart.FirstOrDefault(i => i.Id == product.Id);
            if (cartItem != null)
            {
                cartItem.Quantity += data.Quantity;
            }
            else
            {
                cart.Add(new Item { Id = product.Id, ItemName = product.ItemName, SellingPrice = product.SellingPrice, Quantity = data.Quantity });
            }
            SaveCartToSession(cart);

            // Return the updated cart for a client-side refresh
            return Json(new { success = true, cart = cart });
        }
    }
}
