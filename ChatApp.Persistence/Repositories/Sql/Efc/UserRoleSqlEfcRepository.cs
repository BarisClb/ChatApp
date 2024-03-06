using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Domain.Entities;
using ChatApp.Persistence.Contexts;

namespace ChatApp.Persistence.Repositories.Sql.Efc
{
    public class UserRoleSqlEfcRepository : BaseSqlEfcRepository<UserRole>, IUserRoleSqlEfcRepository
    {
        public UserRoleSqlEfcRepository(ChatAppDbContext context) : base(context)
        { }
    }
}
