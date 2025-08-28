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
    public class LoanRepository : GenericDapperRepository<Loan>, ILoanRepository
    {
        private readonly IDapperContext _context;

        public LoanRepository(IDapperContext context) : base(context, "Loans")
        {
            _context = context;
        }

        public async Task RecordLoanPayment(LoanPayment payment)
        {
            var sql = @"
                INSERT INTO LoanPayments (LoanId, PaymentDate, Amount, Notes)
                VALUES (@LoanId, @PaymentDate, @Amount, @Notes);";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, payment);
            }
        }

        public async Task<IEnumerable<LoanPayment>> GetLoanPayments(int loanId)
        {
            var sql = "SELECT * FROM LoanPayments WHERE LoanId = @LoanId ORDER BY PaymentDate DESC";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<LoanPayment>(sql, new { LoanId = loanId });
            }
        }
    }
}