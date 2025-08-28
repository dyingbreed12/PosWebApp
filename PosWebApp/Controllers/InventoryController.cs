using Microsoft.AspNetCore.Mvc;
using PosWebAppBusinessLogic.Interfaces;


public class InventoryController : Controller
{
    private readonly IProductRepository _productRepository;

    public InventoryController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task <IActionResult> Index()
    {
        var products = await _productRepository.GetAllProducts();

        return View(products);
    }
}