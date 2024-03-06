using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Domain.Enums;
using MediatR;

namespace ChatApp.Application.Queries.UserRole
{
    public class GetUserRoleByUserIdQuery : IRequest<GetUserRoleByUserIdQueryData>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserRoleByUserIdQueryHandler : IRequestHandler<GetUserRoleByUserIdQuery, GetUserRoleByUserIdQueryData>
    {
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;

        public GetUserRoleByUserIdQueryHandler(IBaseSqlDapperRepository baseSqlDapperRepository)
        {
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
        }


        public async Task<GetUserRoleByUserIdQueryData> Handle(GetUserRoleByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return null;

            return await _sqlDapperRepository.QueryFirstOrDefaultAsync<GetUserRoleByUserIdQueryData>(SqlQueryHelper.GetUserRoleByUserIdQuery, new { EntityStatusTypeActive = (int)EntityStatusType.Active, UserId = request.UserId });
        }
    }

    public class GetUserRoleByUserIdQueryData
    {
        public Guid UserId { get; set; }
        public EntityStatusType Status { get; set; }
        public bool IsAdmin { get; set; }
    }
}
