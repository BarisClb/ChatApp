using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Domain.Enums;
using MediatR;

namespace ChatApp.Application.Queries.User
{
    public class GetUserByIdForTokenQuery : IRequest<GetUserByIdForTokenQueryData>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserByIdForTokenQueryHandler : IRequestHandler<GetUserByIdForTokenQuery, GetUserByIdForTokenQueryData>
    {
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;
        private readonly ILanguageHelper _languageHelper;

        public GetUserByIdForTokenQueryHandler(IBaseSqlDapperRepository baseSqlDapperRepository, ILanguageHelper languageHelper)
        {
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
            _languageHelper = languageHelper ?? throw new ArgumentNullException(nameof(languageHelper));
        }


        public async Task<GetUserByIdForTokenQueryData> Handle(GetUserByIdForTokenQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
                return null;

            return await _sqlDapperRepository.QueryFirstOrDefaultAsync<GetUserByIdForTokenQueryData>(SqlQueryHelper.GetUserByIdForTokenQuery, new { EntityStatusTypeActive = (int)EntityStatusType.Active, UserId = request.UserId });
        }
    }

    public class GetUserByIdForTokenQueryData
    {
        public int Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string LanguageCode { get; set; }
        public UserStatusType UserStatus { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
