// CounterController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Security.Claims;
using PosWebAppCommon.Models;
using PosWebAppBusinessLogic.Interfaces;

namespace PosWebApp.Controllers
{
    [Authorize(Roles = "Counter,Admin")]
    public class CounterController : Controller
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISalesRepository _salesRepository;
        private readonly IProductRepository  _productRepository;

        public CounterController(IInventoryRepository inventoryRepository, ISalesRepository salesRepository, IProductRepository productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _salesRepository = salesRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            ViewBag.Cart = cart;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string itemName)
        {
            var product = await  _productRepository.GetProductByName(itemName);
            if (product != null && product.Quantity > 0)
            {
                var cart = GetCartFromSession();
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

        [HttpPost]
        public async Task<IActionResult> CompleteTransaction()
        {
            var cart = GetCartFromSession();
            if (cart.Any())
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var totalAmount = cart.Sum(item => item.Price * item.Quantity);

                var transactionId = await _salesRepository.CreateSalesTransaction(totalAmount, userId);

                var transactionItems = cart.Select(item => new TransactionItem
                {
                    TransactionId = transactionId,
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    PricePerItem = item.Price,
                    UserId = userId
                });

                await _salesRepository.AddTransactionItems(transactionItems);

                foreach (var item in cart)
                {
                   await _productRepository.DecrementProductQuantity(item.ItemId, item.Quantity);
                }

                HttpContext.Session.Remove("Cart");
            }

            return RedirectToAction("Index");
        }

        private List<Product> GetCartFromSession()
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