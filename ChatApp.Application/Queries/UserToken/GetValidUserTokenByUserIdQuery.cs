using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Domain.Enums;
using MediatR;

namespace ChatApp.Application.Queries.UserToken
{
    public class GetValidUserTokenByUserIdQuery : IRequest<GetValidUserTokenByUserIdQueryData>
    {
        public int UserId { get; set; }
    }

    public class GetValidUserTokenByUserIdQueryHandler : IRequestHandler<GetValidUserTokenByUserIdQuery, GetValidUserTokenByUserIdQueryData>
    {
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;

        public GetValidUserTokenByUserIdQueryHandler(IBaseSqlDapperRepository baseSqlDapperRepository)
        {
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
        }


        public async Task<GetValidUserTokenByUserIdQueryData> Handle(GetValidUserTokenByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == 0)
                return null;

            return await _sqlDapperRepository.QueryFirstOrDefaultAsync<GetValidUserTokenByUserIdQueryData>(SqlQueryHelper.GetValidUserTokenByUserIdQuery, new { EntityStatusTypeActive = (int)EntityStatusType.Active, UserId = request.UserId, DateTimeUtcNow = DateTime.UtcNow });
        }
    }

    public class GetValidUserTokenByUserIdQueryData
    {
        public int UserId { get; set; }
        public Guid RefreshTokenId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
