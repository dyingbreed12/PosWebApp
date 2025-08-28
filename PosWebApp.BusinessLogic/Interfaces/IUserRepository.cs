using PosWebAppBusinessLogic.Common;
using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Interfaces
{
    public interface IUserRepository : IGenericDapperRepository<User>
    {
        Task<User?> GetUserByUsername(string username);
    }
}