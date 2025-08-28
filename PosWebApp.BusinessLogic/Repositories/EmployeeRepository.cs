using Dapper;
using PosWebAppBusinessLogic.Common;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Interfaces;
using PosWebAppCommon.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PosWebAppBusinessLogic.Repositories
{
    public class EmployeeRepository : GenericDapperRepository<Employee>, IEmployeeRepository
    {
        private readonly IDapperContext _context;

        public EmployeeRepository(IDapperContext context) : base(context, "Employees")
        {
            _context = context;
        }

        public async Task AddPaycheck(Paycheck paycheck)
        {
            var sql = @"
                INSERT INTO Paychecks (EmployeeId, PayPeriodStart, PayPeriodEnd, GrossPay, Deductions, NetPay, PayDate)
                VALUES (@EmployeeId, @PayPeriodStart, @PayPeriodEnd, @GrossPay, @Deductions, @NetPay, @PayDate);";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, paycheck);
            }
        }

        public async Task<IEnumerable<Paycheck>> GetPaychecksByEmployeeId(int employeeId)
        {
            var sql = "SELECT * FROM Paychecks WHERE EmployeeId = @EmployeeId ORDER BY PayDate DESC";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Paycheck>(sql, new { EmployeeId = employeeId });
            }
        }
    }
}