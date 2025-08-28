using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IProductRepository _productRepository;

        public SalesController(IInventoryRepository inventoryRepository, ISalesRepository salesRepository, IProductRepository productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _salesRepository = salesRepository;
            _productRepository = productRepository;
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
                var totalAmount = cart.Sum(item => item.Price * item.Quantity);

                // Create the sales transaction
                var transactionId = await _salesRepository.CreateSalesTransaction(totalAmount, userId);

                // Prepare and add transaction items
                var transactionItems = cart.Select(item => new TransactionItem
                {
                    TransactionId = transactionId,
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    PricePerItem = item.Price
                });

                await _salesRepository.AddTransactionItems(transactionItems);

                // Decrement inventory quantity
                foreach (var item in cart)
                {
                   await _productRepository.DecrementProductQuantity(item.ItemId, item.Quantity);
                }

                HttpContext.Session.Remove("Cart");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string itemName)
        {
            var product = await _productRepository.GetProductByName(itemName);
            if (product != null && product.Quantity > 0)
            {
                var cart = GetCartFromSession();
                // Check if the product is already in the cart
                var cartItem = cart.FirstOrDefault(i => i.ItemId == product.ItemId);
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cart.Add(new Product { ItemId = product.ItemId, ItemName = product.ItemName, Price = product.Price, Quantity = 1 });
                }
                SaveCartToSession(cart);
            }

            return RedirectToAction("Index");
        }

        // Helper methods for session management
        private List<Product>? GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson) ? new List<Product>() : JsonSerializer.Deserialize<List<Product>>(cartJson);
        }

        private void SaveCartToSession(List<Product> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }
    }
}
