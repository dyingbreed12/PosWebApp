using PosWebAppBusinessLogic.Common;
using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Interfaces
{
    public interface ILoanRepository : IGenericDapperRepository<Loan>
    {
        Task RecordLoanPayment(LoanPayment payment);
        Task<IEnumerable<LoanPayment>> GetLoanPayments(int loanId);
    }
}