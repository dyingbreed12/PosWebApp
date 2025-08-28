using System.Data;

namespace PosWebAppCommon.Interfaces
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}