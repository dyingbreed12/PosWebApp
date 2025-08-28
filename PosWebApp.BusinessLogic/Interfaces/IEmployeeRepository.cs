using PosWebAppBusinessLogic.Common;
using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Interfaces
{
    public interface IEmployeeRepository : IGenericDapperRepository<Employee>
    {
        Task AddPaycheck(Paycheck paycheck);
        Task<IEnumerable<Paycheck>> GetPaychecksByEmployeeId(int employeeId);
    }
}