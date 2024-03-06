using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Domain.Entities;
using ChatApp.Persistence.Contexts;

namespace ChatApp.Persistence.Repositories.Sql.Efc
{
    public class EmailVerificationSqlEfcRepository : BaseSqlEfcRepository<EmailVerification>, IEmailVerificationSqlEfcRepository
    {
        public EmailVerificationSqlEfcRepository(ChatAppDbContext context) : base(context)
        { }
    }
}