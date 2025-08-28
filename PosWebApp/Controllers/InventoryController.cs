using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Models;

namespace PosWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository _inventoryRepo;

        public InventoryController(IInventoryRepository inventoryRepo)
        {
            _inventoryRepo = inventoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _inventoryRepo.GetAllAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item product)
        {
            if (ModelState.IsValid)
            {
                await _inventoryRepo.AddAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _inventoryRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Item product)
        {
            if (ModelState.IsValid)
            {
                await _inventoryRepo.UpdateAsync(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _inventoryRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _inventoryRepo.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}