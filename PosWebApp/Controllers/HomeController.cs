using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebApp.Models;
using PosWebAppBusinessLogic.Services;
using System.Diagnostics;
using PosWebAppCommon; // Reference the common library for the DTO

namespace PosWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DashboardService _dashboardService;

        public HomeController(ILogger<HomeController> logger, DashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            // Get data from the service layer
            var dashboardData = await _dashboardService.GetDashboardData();

            // Map the DTO to the View Model
            var viewModel = new DashboardViewModel
            {
                TotalSales = dashboardData.TotalSales,
                TotalProfit = dashboardData.TotalProfit,
                TotalTransactions = dashboardData.TotalTransactions,
                LowStockItems = dashboardData.LowStockItems,
                DailySales = dashboardData.DailySales,
                TopSellingItems = dashboardData.TopSellingItems
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}