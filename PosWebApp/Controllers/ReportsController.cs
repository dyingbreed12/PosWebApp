using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebAppBusinessLogic.Services;
using PosWebAppCommon;
using System.Threading.Tasks;
using Rotativa.AspNetCore; // For PDF export (requires NuGet package)
using ClosedXML.Excel; // For Excel export (requires NuGet package)
using System.IO;

namespace PosWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly ReportsService _reportsService;

        public ReportsController(ReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSalesHistory(DateTime? startDate, DateTime? endDate)
        {
            var data = await _reportsService.GetSalesHistory(startDate, endDate);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetInventoryLevels()
        {
            var data = await _reportsService.GetInventoryLevels();
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfitLoss(DateTime? startDate, DateTime? endDate)
        {
            var data = await _reportsService.GetProfitLoss(startDate, endDate);
            return Json(data);
        }

        // Action to export a sales history report to PDF
        [HttpGet]
        public async Task<IActionResult> ExportSalesHistoryPdf(DateTime? startDate, DateTime? endDate)
        {
            var data = await _reportsService.GetSalesHistory(startDate, endDate);
            return new ViewAsPdf("SalesHistoryPdf", data)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                FileName = "SalesHistoryReport.pdf"
            };
        }

        // Action to export a sales history report to Excel
        [HttpGet]
        public async Task<IActionResult> ExportSalesHistoryExcel(DateTime? startDate, DateTime? endDate)
        {
            var data = await _reportsService.GetSalesHistory(startDate, endDate);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sales History");
                worksheet.Cell(1, 1).InsertTable(data);
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesHistoryReport.xlsx");
                }
            }
        }
    }
}