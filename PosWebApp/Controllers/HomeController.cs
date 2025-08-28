// Controllers/HomeController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebApp.Models;
using PosWebAppBusinessLogic.Repositories;
using PosWebAppBusinessLogic.Services;
using System.Diagnostics;

namespace PosWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DashboardRepository _dashboardRepo;
        private readonly DashboardService _dashboardService;

        public HomeController(ILogger<HomeController> logger, DashboardRepository dashboardRepo, DashboardService  dashboardService)
        {
            _logger = logger;
            _dashboardRepo = dashboardRepo;
            _dashboardService = dashboardService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var dashboardData = _dashboardService.GetDashboardData();

            var viewModel = new DashboardViewModel
            {
                TotalSales = dashboardData.TotalSales,
                TotalTransactions = dashboardData.TotalTransactions,
                DailySales = dashboardData.DailySales
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