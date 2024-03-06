using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Application.Models.Data;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Persistence.Repositories.Sql.Efc
{
    public class UserTokenSqlEfcRepository : BaseSqlEfcRepository<UserToken>, IUserTokenSqlEfcRepository
    {
        public UserTokenSqlEfcRepository(ChatAppDbContext context) : base(context)
        { }


        public async Task<int> UserTokenDisableByUserId(Guid userId)
        {
            return await _entity.Where(e => e.UserId == userId).ExecuteUpdateAsync(e => e.SetProperty(e => e.Status, EntityStatusType.Disabled));
        }

        public async Task<int> UserTokenDisableByUserAndRefreshTokenId(Guid userId, Guid refreshTokenId)
        {
            return await _entity.Where(userToken => userToken.UserId == userId && userToken.RefreshTokenId == refreshTokenId).ExecuteUpdateAsync(userToken => userToken.SetProperty(userToken => userToken.Status, EntityStatusType.Disabled));
        }

        public async Task<UserToken> UpdateUserToken(UpdateUserTokenWithEfc newValues)
        {
            var userToken = await _entity.FindAsync(newValues.Id);
            if (userToken == null)
                return null;

            if (newValues.Status != null)
                userToken.Status = newValues.Status ?? default;
            if (newValues.RefreshTokenId != null)
                userToken.RefreshTokenId = newValues.RefreshTokenId ?? default;
            if (newValues.IssueDate != null)
                userToken.IssueDate = newValues.IssueDate ?? default;
            if (newValues.ExpirationDate != null)
                userToken.ExpirationDate = newValues.ExpirationDate ?? default;
            userToken.DateUpdated = DateTime.UtcNow;

            return await SaveChangesAsync() > 0 ? userToken : null;
        }
    }
}
