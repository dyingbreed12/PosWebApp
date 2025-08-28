using Dapper;
using PosWebAppBusinessLogic.Common;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Interfaces;
using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Repositories
{
    public class UserRepository : GenericDapperRepository<User>, IUserRepository
    {
        private readonly IDapperContext _context;

        public UserRepository(IDapperContext context) : base(context, "Users")
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var sql = "SELECT * FROM Users WHERE Username = @Username";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
            }
        }
    }
}