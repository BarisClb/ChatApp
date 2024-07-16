using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using MediatR;

namespace ChatApp.Application.Queries.UserToken
{
    public class GetUserTokenByUserAndRefreshTokenIdQuery : IRequest<GetUserTokenByUserAndRefreshTokenIdQueryResponse>
    {
        public Guid UserId { get; set; }
        public Guid RefreshTokenId { get; set; }
    }

    public class GetUserTokenByUserAndRefreshTokenIdQueryHandler : IRequestHandler<GetUserTokenByUserAndRefreshTokenIdQuery, GetUserTokenByUserAndRefreshTokenIdQueryResponse>
    {
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;

        public GetUserTokenByUserAndRefreshTokenIdQueryHandler(IBaseSqlDapperRepository sqlDapperRepository)
        {
            _sqlDapperRepository = sqlDapperRepository ?? throw new ArgumentNullException(nameof(sqlDapperRepository));
        }


        public async Task<GetUserTokenByUserAndRefreshTokenIdQueryResponse> Handle(GetUserTokenByUserAndRefreshTokenIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty || request.RefreshTokenId == Guid.Empty)
                return null;

            return await _sqlDapperRepository.QueryFirstOrDefaultAsync<GetUserTokenByUserAndRefreshTokenIdQueryResponse>(SqlQueryHelper.GetUserTokenByUserAndRefreshTokenIdQuery, new { UserId = request.UserId, RefreshTokenId = request.RefreshTokenId });
        }
    }

    public class GetUserTokenByUserAndRefreshTokenIdQueryResponse
    {
        public Guid Id { get; set; }
        public Guid RefreshTokenId { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
