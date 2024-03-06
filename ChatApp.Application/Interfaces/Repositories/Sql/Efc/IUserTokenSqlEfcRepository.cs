using ChatApp.Application.Models.Data;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Repositories.Sql.Efc
{
    public interface IUserTokenSqlEfcRepository : IBaseSqlEfcRepository<UserToken>
    {
        Task<int> UserTokenDisableByUserId(Guid userId);
        Task<int> UserTokenDisableByUserAndRefreshTokenId(Guid userId, Guid refreshTokenId);
        Task<UserToken> UpdateUserToken(UpdateUserTokenWithEfc newValues);
    }
}
