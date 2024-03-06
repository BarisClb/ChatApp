using ChatApp.Application.Configurations.Pipelines;
using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Domain.Enums;
using MediatR;

namespace ChatApp.Application.Queries.User
{
    public class GetUserAndUserRoleByUserIdQuery : IRequest<GetUserAndUserRoleByUserIdQueryData>, ICacheableQuery
    {
        public Guid UserId { get; set; }

        public string CacheKey => "testtt";

        public TimeSpan? ExpirationTime => null;
    }

    public class GetUserAndUserRoleByUserIdQueryHandler : IRequestHandler<GetUserAndUserRoleByUserIdQuery, GetUserAndUserRoleByUserIdQueryData>
    {
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;

        public GetUserAndUserRoleByUserIdQueryHandler(IBaseSqlDapperRepository baseSqlDapperRepository)
        {
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
        }


        public async Task<GetUserAndUserRoleByUserIdQueryData> Handle(GetUserAndUserRoleByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return null;

            return await _sqlDapperRepository.QueryFirstOrDefaultAsync<GetUserAndUserRoleByUserIdQueryData>(SqlQueryHelper.GetUserAndUserRoleByUserIdQuery, new { EntityStatusTypeActive = (int)EntityStatusType.Active, UserId = request.UserId });
        }
    }

    public class GetUserAndUserRoleByUserIdQueryData
    {
        public Guid UserId { get; set; }
        public UserStatusType UserUserStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string LanguageCode { get; set; }
        public DateTime UserDateCreated { get; set; }
        public bool IsAdmin { get; set; }
    }
}
