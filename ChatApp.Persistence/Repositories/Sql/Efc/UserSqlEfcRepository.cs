using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Domain.Entities;
using ChatApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;
using System.Linq;
using ChatApp.Domain.Enums;

namespace ChatApp.Persistence.Repositories.Sql.Efc
{
    public class UserSqlEfcRepository : BaseSqlEfcRepository<User>, IUserSqlEfcRepository
    {
        public UserSqlEfcRepository(ChatAppDbContext context) : base(context)
        { }


        public async Task<bool> DeleteAsync(Guid entityId)
        {
            return (await _entity.Where(x => x.Id == entityId).ExecuteDeleteAsync()) > 0;
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<Guid> entities)
        {
            return await _entity.Where(x => entities.Contains(x.Id)).ExecuteDeleteAsync();
        }


        public async Task<bool> DisableByIdAsync(Guid entityId)
        {
            return await _entity.Where(e => e.Id == entityId).ExecuteUpdateAsync(e => e.SetProperty(e => e.Status, EntityStatusType.Disabled)) > 0;
        }

        public async Task<int> DisableRangeByIdAsync(IEnumerable<Guid> entities)
        {
            return await _entity.Where(e => entities.Contains(e.Id)).ExecuteUpdateAsync(e => e.SetProperty(e => e.Status, EntityStatusType.Disabled));
        }
    }
}
