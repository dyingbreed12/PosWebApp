using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosWebAppBusinessLogic.Services
{
    public class PayrollService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public PayrollService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task CalculateAndSavePaycheck(int employeeId, DateTime payPeriodStart, DateTime payPeriodEnd)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null) return;

            // Simple calculation for a fixed salary.
            var grossPay = employee.Salary;
            // You can add more complex logic for deductions here if needed.
            var deductions = 0m;
            var netPay = grossPay - deductions;

            var paycheck = new Paycheck
            {
                EmployeeId = employee.Id,
                PayPeriodStart = payPeriodStart,
                PayPeriodEnd = payPeriodEnd,
                GrossPay = grossPay,
                Deductions = deductions,
                NetPay = netPay,
                PayDate = DateTime.Now
            };

            await _employeeRepository.AddPaycheck(paycheck);
        }
    }
}
