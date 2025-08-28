using Dapper;
using PosWebAppCommon.Interfaces;
using System.Data;
using System.Reflection;
using System.Text;

namespace PosWebAppBusinessLogic.Common
{
    public class GenericDapperRepository<T> : IGenericDapperRepository<T> where T : class
    {
        private readonly IDapperContext _context;
        protected readonly string _tableName;

        public GenericDapperRepository(IDapperContext context, string tableName)
        {
            _context = context;
            _tableName = tableName;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {_tableName}";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<T>(sql);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
            }
        }

        public async Task<int> AddAsync(T entity)
        {
            var properties = GetProperties(entity);
            var sql = BuildInsertQuery(_tableName, properties);
            using (var connection = _context.CreateConnection())
            {
                // ExecuteScalarAsync returns the Id of the newly inserted row
                return await connection.ExecuteAsync(sql, entity);
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var properties = GetProperties(entity);
            var sql = BuildUpdateQuery(_tableName, properties);
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, entity);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
        }

        private static IEnumerable<PropertyInfo> GetProperties(T entity)
        {
            return typeof(T).GetProperties().Where(p => p.Name != "Id");
        }

        private static string BuildInsertQuery(string tableName, IEnumerable<PropertyInfo> properties)
        {
            var sb = new StringBuilder($"INSERT INTO {tableName} (");
            var values = new StringBuilder("VALUES (");
            var isFirst = true;

            foreach (var prop in properties)
            {
                if (!isFirst)
                {
                    sb.Append(", ");
                    values.Append(", ");
                }
                sb.Append(prop.Name);
                values.Append($"@{prop.Name}");
                isFirst = false;
            }

            sb.Append(") ");
            values.Append(")");

            return sb.ToString() + values.ToString();
        }

        private static string BuildUpdateQuery(string tableName, IEnumerable<PropertyInfo> properties)
        {
            var sb = new StringBuilder($"UPDATE {tableName} SET ");
            var isFirst = true;

            foreach (var prop in properties)
            {
                if (!isFirst)
                {
                    sb.Append(", ");
                }
                sb.Append($"{prop.Name} = @{prop.Name}");
                isFirst = false;
            }

            sb.Append(" WHERE Id = @Id");
            return sb.ToString();
        }
    }
}