using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Repositories.Sql.Efc
{
    public interface IUserSqlEfcRepository : IBaseSqlEfcRepository<User>
    {
        Task<bool> DeleteAsync(Guid entityId);
        Task<int> DeleteRangeAsync(IEnumerable<Guid> entities);

        Task<bool> DisableByIdAsync(Guid entityId);
        Task<int> DisableRangeByIdAsync(IEnumerable<Guid> entities);
    }
}
