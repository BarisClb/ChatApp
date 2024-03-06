using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ChatApp.Persistence.Repositories.Sql.Dapper
{
    public class MsSqlDapperRepository : IBaseSqlDapperRepository
    {
        private readonly string _sqlConnectionString;

        public MsSqlDapperRepository(IConfiguration configuration)
        {
            _sqlConnectionString = configuration.GetConnectionString("MsSql") ?? throw new Exception("ConnectionString was not provided for MsSqlDapperRepository.");
        }


        public async Task<TEntity> QueryFirstOrDefaultAsync<TEntity>(string sqlQuery, object param)
        {
            using var connection = await CreateConnection();
            return (await connection.QueryAsync<TEntity>(sqlQuery, param)).FirstOrDefault();
        }

        public async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sqlQuery, object param)
        {
            using var connection = await CreateConnection();
            return await connection.QueryAsync<TEntity>(sqlQuery, param);
        }

        public async Task<T> QuerySingleAsync<T>(string sqlQuery, object param)
        {
            using var connection = await CreateConnection();
            return await connection.QuerySingleAsync<T>(sqlQuery, param);
        }

        public async Task<int> ExecuteAsync(string sqlQuery, object param)
        {
            using var connection = await CreateConnection();
            return await connection.ExecuteAsync(sqlQuery, param);
        }

        public async Task<int> ExistsAsync(string sqlQuery, object param)
        {
            using var connection = await CreateConnection();
            return await connection.ExecuteAsync(sqlQuery, param);
        }


        private async Task<SqlConnection> CreateConnection()
        {
            SqlConnection connection = new(_sqlConnectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            return connection;
        }
    }
}
