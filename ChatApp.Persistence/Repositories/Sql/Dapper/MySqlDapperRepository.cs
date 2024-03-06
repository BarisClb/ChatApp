using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace ChatApp.Persistence.Repositories.Sql.Dapper
{
    public class MySqlDapperRepository : IBaseSqlDapperRepository
    {
        private readonly string _sqlConnectionString;

        public MySqlDapperRepository(IConfiguration configuration)
        {
            _sqlConnectionString = configuration.GetConnectionString("MySql") ?? throw new Exception("ConnectionString was not provided for MySqlDapperRepository.");
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


        private async Task<MySqlConnection> CreateConnection()
        {
            MySqlConnection connection = new(_sqlConnectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            return connection;
        }
    }
}
