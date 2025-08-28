using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Interfaces
{
    public interface ISalesRepository
    {
        Task<int> CreateSalesTransaction(decimal totalAmount, int userId);
        Task AddTransactionItems(IEnumerable<TransactionItem> items);
    }
}
