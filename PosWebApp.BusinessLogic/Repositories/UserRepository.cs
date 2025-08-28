using Dapper;
using PosWebAppBusinessLogic.Interfaces;
using PosWebAppCommon.Interfaces;
using PosWebAppCommon.Models;

namespace PosWebAppBusinessLogic.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDapperContext _context;

        public UserRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var sql = "SELECT * FROM Users WHERE Username = @username AND IsActive = 1";
            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QueryAsync<User>(sql, new { username });

                return user.FirstOrDefault(); 
            }
        }

        public async Task<int> CreateAsync(User user)
        {
            var sql = @"INSERT INTO Users (Username, PasswordHash, Role) 
                        VALUES (@Username, @PasswordHash, @Role);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            using (var connection = _context.CreateConnection())
            {
               return await connection.ExecuteScalarAsync<int>(sql, user);
            }
        }
    }
}
