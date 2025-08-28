using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebApp.Models;
using PosWebApp.Models.ViewModel;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.DTOs;
using PosWebAppCommon.Models;
using System.Security.Claims;
using System.Text.Json;

namespace PosWebApp.Controllers
{
    [Authorize(Roles = "Counter, Admin")]
    public class SalesController : Controller
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISalesRepository _salesRepository;

        public SalesController(IInventoryRepository inventoryRepository, ISalesRepository salesRepository)
        {
            _inventoryRepository = inventoryRepository;
            _salesRepository = salesRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cart = GetCartFromSession();
            var initialProducts = await _inventoryRepository.GetAllAsync();

            var viewModel = new SalesViewModel
            {
                Products = initialProducts.ToList(),
                Cart = cart.ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> SearchItem(string query)
        {
            var items = await _inventoryRepository.SearchItems(query);
            return Json(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto data)
        {
            if (data == null || string.IsNullOrEmpty(data.Sku) || data.Quantity <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }

            var item = await _inventoryRepository.GetProductBySku(data.Sku);
            if (item == null)
            {
                return NotFound(new { success = false, message = "Item not found." });
            }

            if (item.Quantity < data.Quantity)
            {
                return BadRequest(new { success = false, message = $"Not enough stock for {item.ItemName}. Available: {item.Quantity}." });
            }

            var cart = GetCartFromSession();
            var cartItem = cart.FirstOrDefault(i => i.Id == item.Id);

            if (cartItem != null)
            {
                cartItem.Quantity += data.Quantity;
                if (cartItem.Quantity > item.Quantity)
                {
                    return BadRequest(new { success = false, message = "Cannot add more than available stock." });
                }
            }
            else
            {
                cart.Add(new Item { Id = item.Id, ItemName = item.ItemName, SellingPrice = item.SellingPrice, Quantity = data.Quantity });
            }

            SaveCartToSession(cart);
            return Ok(new { success = true, cart = cart });
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] RemoveFromCartDto data)
        {
            if (data == null || data.ItemId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }

            var cart = GetCartFromSession();
            var itemToRemove = cart.FirstOrDefault(i => i.Id == data.ItemId);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                SaveCartToSession(cart);
                return Ok(new { success = true, cart = cart });
            }

            return NotFound(new { success = false, message = "Item not found in cart." });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityDto data)
        {
            if (data == null || data.ItemId <= 0 || data.Quantity <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid data." });
            }

            var itemInDb = await _inventoryRepository.GetByIdAsync(data.ItemId);
            if (itemInDb == null)
            {
                return NotFound(new { success = false, message = "Item not found in database." });
            }

            if (itemInDb.Quantity < data.Quantity)
            {
                return BadRequest(new { success = false, message = $"Cannot update quantity. Only {itemInDb.Quantity} available." });
            }

            var cart = GetCartFromSession();
            var itemToUpdate = cart.FirstOrDefault(i => i.Id == data.ItemId);

            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = data.Quantity;
                SaveCartToSession(cart);
                return Ok(new { success = true, cart = cart });
            }

            return NotFound(new { success = false, message = "Item not found in cart." });
        }

        [HttpPost]
        public async Task<IActionResult> CompleteTransaction()
        {
            var cart = GetCartFromSession();

            if (!cart.Any())
            {
                return BadRequest(new { success = false, message = "Cart is empty." });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { success = false, message = "User not authenticated." });
            }

            var totalAmount = cart.Sum(item => item.SellingPrice * item.Quantity);

            var transactionId = await _salesRepository.CreateSalesTransaction(totalAmount, userId);

            var transactionItems = cart.Select(item => new TransactionItem
            {
                TransactionId = transactionId,
                ItemId = item.Id,
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                PricePerItem = item.SellingPrice
            });

            await _salesRepository.AddTransactionItems(transactionItems);

            foreach (var item in cart)
            {
                await _inventoryRepository.DecrementProductQuantity(item.Id, item.Quantity);
            }

            HttpContext.Session.Remove("Cart");
            return Ok(new { success = true, message = "Transaction completed." });
        }

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
    }
}