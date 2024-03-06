using ChatApp.Domain.Entities.Common;

namespace ChatApp.Application.Interfaces.Repositories.Sql.Efc
{
    public interface IBaseSqlEfcRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<int> AddRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);
        Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities);

        Task<bool> DeleteAsync(int entityId);
        Task<bool> DeleteAsync(TEntity entity);
        Task<int> DeleteRangeAsync(IEnumerable<int> entities);
        Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities);

        Task<bool> DisableByIdAsync(int entityId);
        Task<int> DisableRangeByIdAsync(IEnumerable<int> entities);

        // Helpers

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
