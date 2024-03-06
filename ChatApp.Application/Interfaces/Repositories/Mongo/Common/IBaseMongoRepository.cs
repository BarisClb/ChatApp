using System.Linq.Expressions;

namespace ChatApp.Application.Interfaces.Repositories.Mongo.Common
{
    public interface IBaseMongoRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
        Task InsertAsync(TEntity entity);
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
