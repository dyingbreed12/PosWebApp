using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppBusinessLogic.Services;
using PosWebAppCommon.Models;

namespace PosWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PayrollController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly PayrollService _payrollService;

        public PayrollController(IEmployeeRepository employeeRepository, PayrollService payrollService)
        {
            _employeeRepository = employeeRepository;
            _payrollService = payrollService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> CalculatePaycheck(int employeeId)
        {
            // You can add logic to get the start and end dates from a form here
            await _payrollService.CalculateAndSavePaycheck(employeeId, DateTime.Now.AddMonths(-1), DateTime.Now);
            return RedirectToAction("Index");
        }

        // Action to get a view for adding a new employee
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Position,Salary,IsActive")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepository.AddAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // Action to get a view for editing an employee
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,Position,Salary,IsActive")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _employeeRepository.UpdateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // Action to handle employee deletion (soft delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Action to view an employee's paycheck history
        public async Task<IActionResult> PaycheckHistory(int employeeId)
        {
            var paychecks = await _employeeRepository.GetPaychecksByEmployeeId(employeeId);
            return View(paychecks);
        }
    }
}