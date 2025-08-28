using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppBusinessLogic.Services;
using PosWebAppCommon.Models;
using System.Threading.Tasks;

namespace PosWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LendingController : Controller
    {
        private readonly ILoanRepository _loanRepository;
        private readonly LendingService _lendingService;

        public LendingController(ILoanRepository loanRepository, LendingService lendingService)
        {
            _loanRepository = loanRepository;
            _lendingService = lendingService;
        }

        public async Task<IActionResult> Index()
        {
            var loans = await _loanRepository.GetAllAsync();
            return View(loans);
        }

        // Action to view loan details and payment history
        public async Task<IActionResult> Details(int id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            var payments = await _loanRepository.GetLoanPayments(id);
            ViewBag.Payments = payments;
            return View(loan);
        }

        // Action to get a view for creating a new loan
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Loan loan)
        {
            if (ModelState.IsValid)
            {
                loan.LoanDate = DateTime.Now;
                loan.Balance = loan.TotalAmount;
                loan.Status = "Active";
                await _loanRepository.AddAsync(loan);
                return RedirectToAction(nameof(Index));
            }
            return View(loan);
        }

        // Action to get a view for editing an existing loan
        public async Task<IActionResult> Edit(int id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            return View(loan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Loan loan)
        {
            if (ModelState.IsValid)
            {
                await _loanRepository.UpdateAsync(loan);
                return RedirectToAction(nameof(Index));
            }
            return View(loan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakePayment(int loanId, decimal paymentAmount)
        {
            if (paymentAmount <= 0)
            {
                TempData["ErrorMessage"] = "Payment amount must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }

            await _lendingService.ProcessPayment(loanId, paymentAmount);
            TempData["SuccessMessage"] = "Payment recorded successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}