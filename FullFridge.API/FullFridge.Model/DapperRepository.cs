using Dapper;
using Npgsql;
using System.Data;

namespace FullFridge.Model
{
    public class DapperRepository: IDapperRepository
    {
        private readonly IDbConnection _connection;

        public DapperRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Execute(string query, object parameters = null)
        {
            using var con = Connection;
            return await con.ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<T>> Query<T>(string query,object parameters = null)
        {
            using var con = Connection;
            return await con.QueryAsync<T>(query, parameters);
        }

        public async Task<T> QueryFirstOrDefault<T>(string query, object parameters = null)
        {
            using var con = Connection;
            return await con.QueryFirstOrDefaultAsync<T>(query, parameters);
        }

        private IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_connection.ConnectionString);
            }
        }
    }

    public interface IDapperRepository
    {
        Task<int> Execute(string query, object parameters = null);
        Task<IEnumerable<T>> Query<T>(string query, object parameters = null);
        Task<T> QueryFirstOrDefault<T>(string query, object parameters = null);
    }
}