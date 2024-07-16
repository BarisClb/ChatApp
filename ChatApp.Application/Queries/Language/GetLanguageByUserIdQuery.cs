using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Domain.Enums;
using MediatR;

namespace ChatApp.Application.Queries.Language
{
    public class GetLanguageByUserIdQuery : IRequest<GetLanguageByUserIdQueryData>
    {
        public Guid UserId { get; set; }
    }

    public class GetLanguageByUserIdQueryHandler : IRequestHandler<GetLanguageByUserIdQuery, GetLanguageByUserIdQueryData>
    {
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;

        public GetLanguageByUserIdQueryHandler(IBaseSqlDapperRepository baseSqlDapperRepository)
        {
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
        }


        public async Task<GetLanguageByUserIdQueryData> Handle(GetLanguageByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return null;

            return await _sqlDapperRepository.QueryFirstOrDefaultAsync<GetLanguageByUserIdQueryData>(SqlQueryHelper.GetLanguageByUserIdQuery, new { EntityStatusTypeActive = (int)EntityStatusType.Active, UserId = request.UserId });
        }
    }

    public class GetLanguageByUserIdQueryData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
