using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace ChatApp.Persistence.Repositories.Sql.Dapper
{
    public class PostgreSqlDapperRepository : IBaseSqlDapperRepository
    {
        private readonly string _sqlConnectionString;

        public PostgreSqlDapperRepository(IConfiguration configuration)
        {
            _sqlConnectionString = configuration.GetConnectionString("PostgreSql") ?? throw new Exception("ConnectionString was not provided for PostgreSqlDapperRepository.");
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


        private async Task<NpgsqlConnection> CreateConnection()
        {
            NpgsqlConnection connection = new(_sqlConnectionString);
            if (connection.State == ConnectionState.Closed) connection.Open();
            return connection;
        }
    }
}
