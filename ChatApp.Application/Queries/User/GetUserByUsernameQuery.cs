using ChatApp.Application.Configurations.Pipelines;
using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Application.Models.Responses.Common;
using ChatApp.Application.Resources;
using ChatApp.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;

namespace ChatApp.Application.Queries.User
{
    public class GetUserByUsernameQuery : IRequest<ApiResponse<GetUserByUsernameQueryData>>, ICacheableQuery
    {
        public string Username { get; set; }

        public string CacheKey => string.Format(CacheHelper.UserByUsernameKey, Username);
        public TimeSpan? ExpirationTime => CacheHelper.UserByUsernameExpirationTime;
    }


    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, ApiResponse<GetUserByUsernameQueryData>>
    {
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;
        private readonly IStringLocalizer<LocalizationResources> _localizer;

        public GetUserByUsernameQueryHandler(IBaseSqlDapperRepository baseSqlDapperRepository, IStringLocalizer<LocalizationResources> localizer)
        {
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }


        public async Task<ApiResponse<GetUserByUsernameQueryData>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Username))
                return ApiResponse<GetUserByUsernameQueryData>.Fail(string.Format(_localizer[LocalizationKeys.FollowingFieldWasNotProvidedForTheFollowingMethod], "Username", "GetUserByUsernameQueryData"), (int)HttpStatusCode.BadRequest);

            var user = await _sqlDapperRepository.QueryFirstOrDefaultAsync<GetUserByUsernameQueryData>(SqlQueryHelper.GetUserByUsernameQuery, new { EntityStatusTypeActive = (int)EntityStatusType.Active, Username = request.Username });
            if (user == null)
                return ApiResponse<GetUserByUsernameQueryData>.Fail(string.Format(_localizer[LocalizationKeys.FailedToRetrieveFollowingData], "User"), (int)HttpStatusCode.NotFound);

            return ApiResponse<GetUserByUsernameQueryData>.Success(user, (int)HttpStatusCode.OK);
        }
    }

    public class GetUserByUsernameQueryData
    {
        public Guid Id { get; set; }
        public EntityStatusType Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserStatusType UserStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
