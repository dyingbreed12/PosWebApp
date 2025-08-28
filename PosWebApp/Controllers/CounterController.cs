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

        public CounterController(IInventoryRepository inventoryRepository, ISalesRepository salesRepository)
        {
            _inventoryRepository = inventoryRepository;
            _salesRepository = salesRepository;
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
            var product = await _inventoryRepository.GetProductByName(itemName);
            if (product != null && product.Quantity > 0)
            {
                var cart = GetCartFromSession();
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

        [HttpPost]
        public async Task<IActionResult> CompleteTransaction()
        {
            var cart = GetCartFromSession();
            if (cart.Any())
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var totalAmount = cart.Sum(item => item.SellingPrice * item.Quantity);

                var transactionId = await _salesRepository.CreateSalesTransaction(totalAmount, userId);

                var transactionItems = cart.Select(item => new TransactionItem
                {
                    TransactionId = transactionId,
                    ItemId = item.Id,
                    ItemName = item.ItemName,
                    Quantity = item.Quantity,
                    PricePerItem = item.SellingPrice,
                    UserId = userId
                });

                await _salesRepository.AddTransactionItems(transactionItems);

                foreach (var item in cart)
                {
                   await _inventoryRepository.DecrementProductQuantity(item.Id, item.Quantity);
                }

                HttpContext.Session.Remove("Cart");
            }

            return RedirectToAction("Index");
        }

        private List<Item> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson) ? new List<Item>() : JsonSerializer.Deserialize<List<Item>>(cartJson);
        }

        private void SaveCartToSession(List<Item> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }
    }
}