using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Domain.Entities;
using ChatApp.Persistence.Contexts;

namespace ChatApp.Persistence.Repositories.Sql.Efc
{
    public class LanguageSqlEfcRepository : BaseSqlEfcRepository<Language>, ILanguageSqlEfcRepository
    {
        public LanguageSqlEfcRepository(ChatAppDbContext context) : base(context)
        { }
    }
}
