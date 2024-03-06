namespace ChatApp.Application.Interfaces.Repositories.Sql.Dapper
{
    public interface IBaseSqlDapperRepository
    {
        Task<TEntity> QueryFirstOrDefaultAsync<TEntity>(string sqlQuery, object param);
        Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string sqlQuery, object param);
        Task<T> QuerySingleAsync<T>(string sqlQuery, object param);
        Task<int> ExecuteAsync(string sqlQuery, object param);
    }
}
