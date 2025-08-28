using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Services
{
    public class LendingService
    {
        private readonly ILoanRepository _loanRepository;

        public LendingService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task ProcessPayment(int loanId, decimal paymentAmount)
        {
            var loan = await _loanRepository.GetByIdAsync(loanId);
            if (loan == null) return;

            loan.Balance -= paymentAmount;

            var payment = new LoanPayment
            {
                LoanId = loanId,
                Amount = paymentAmount,
                PaymentDate = DateTime.Now
            };

            await _loanRepository.RecordLoanPayment(payment);

            if (loan.Balance <= 0)
            {
                loan.Balance = 0;
                loan.Status = "Paid";
            }

            await _loanRepository.UpdateAsync(loan);
        }
    }
}
