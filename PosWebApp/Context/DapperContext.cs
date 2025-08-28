using Microsoft.Data.SqlClient;
using PosWebAppCommon.Interfaces;
using System.Data;

namespace PosWebApp.Context
{
    public class DapperContext(IConfiguration configuration) : IDapperContext
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
