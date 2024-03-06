using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using MediatR;

namespace ChatApp.Application.Commands.UserToken
{
    public class UserTokenRefreshOrCreateCommand : IRequest<Domain.Entities.UserToken>
    {
        public Guid UserId { get; set; }
        public Guid RefreshTokenId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? IssueDate { get; set; }
    }

    public class UserTokenRefreshOrCreateCommandHandler : IRequestHandler<UserTokenRefreshOrCreateCommand, Domain.Entities.UserToken>
    {
        private readonly IUserTokenSqlEfcRepository _userTokenSqlEfcRepository;
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;

        public UserTokenRefreshOrCreateCommandHandler(IUserTokenSqlEfcRepository userTokenSqlEfcRepository, IBaseSqlDapperRepository sqlDapperRepository)
        {
            _userTokenSqlEfcRepository = userTokenSqlEfcRepository ?? throw new ArgumentNullException(nameof(userTokenSqlEfcRepository));
            _sqlDapperRepository = sqlDapperRepository ?? throw new ArgumentNullException(nameof(sqlDapperRepository));
        }


        public async Task<Domain.Entities.UserToken> Handle(UserTokenRefreshOrCreateCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == default || request.RefreshTokenId == default || request.ExpirationDate == default)
                return null;

            var userTokenId = await _sqlDapperRepository.QueryFirstOrDefaultAsync<int?>(SqlQueryHelper.UserTokenRefreshOrCreateQuery, new { UserId = request.UserId, RefreshTokenId = request.RefreshTokenId });

            if (userTokenId != null)
                return await _userTokenSqlEfcRepository.UpdateUserToken(new Models.Data.UpdateUserTokenWithEfc { Id = userTokenId ?? 0, RefreshTokenId = request.RefreshTokenId, ExpirationDate = request.ExpirationDate, Status = Domain.Enums.EntityStatusType.Active });

            if (request.IssueDate == default)
                return null;

            return await _userTokenSqlEfcRepository.AddAsync(new() { UserId = request.UserId, RefreshTokenId = request.RefreshTokenId, IssueDate = request.IssueDate ?? DateTime.UtcNow, ExpirationDate = request.ExpirationDate });
        }
    }
}
