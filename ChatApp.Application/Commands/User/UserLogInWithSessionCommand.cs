using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Responses;
using ChatApp.Application.Models.Responses.Common;
using ChatApp.Application.Resources;
using ChatApp.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Net;

namespace ChatApp.Application.Commands.User
{
    public class UserLogInWithSessionCommand : IRequest<ApiResponse<UserSessionResponse>>
    {
        public string UserField { get; set; }
        public string Password { get; set; }
    }

    public class UserLogInWithSessionCommandHandler : IRequestHandler<UserLogInWithSessionCommand, ApiResponse<UserSessionResponse>>
    {
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;
        private readonly ITokenService _tokenService;
        private readonly IStringLocalizer<LocalizationResources> _localizer;

        public UserLogInWithSessionCommandHandler(IBaseSqlDapperRepository baseSqlDapperRepository, ITokenService tokenService, IStringLocalizer<LocalizationResources> localizer)
        {
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }


        public async Task<ApiResponse<UserSessionResponse>> Handle(UserLogInWithSessionCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserField))
                return ApiResponse<UserSessionResponse>.Fail(_localizer[LocalizationKeys.UserLogInUserFieldCantBeEmpty], (int)HttpStatusCode.BadRequest);
            if (string.IsNullOrEmpty(request.Password))
                return ApiResponse<UserSessionResponse>.Fail(string.Format(_localizer[LocalizationKeys.FollowingFieldWasNotProvided], _localizer[LocalizationKeys.Password]), (int)HttpStatusCode.BadRequest);

            string userFieldQuery = ValidationHelper.CheckIfStringIsInEmailFormat(request.UserField) ? "EmailAddress" : "Username";

            var user = await _sqlDapperRepository.QueryFirstOrDefaultAsync<UserLogInCommandWithSessionData>(string.Format(SqlQueryHelper.UserLogInCommandQuery, userFieldQuery), new { UserField = request.UserField, Password = SecurityHelper.GetSha256Hash(request.Password) });

            if (user == null)
                return ApiResponse<UserSessionResponse>.Fail(_localizer[LocalizationKeys.UserLogInUserWasNotFound], (int)HttpStatusCode.BadRequest);
            if (user.UserStatus == UserStatusType.Inactive)
                return ApiResponse<UserSessionResponse>.Fail(_localizer[LocalizationKeys.UserLogInUserIsInactive], (int)HttpStatusCode.BadRequest);

            var tokens = await _tokenService.GenerateTokens(user.Id);

            return ApiResponse<UserSessionResponse>.Success(ModelMapHelper.MapTokenDataToUserSessionResponse(tokens), (int)HttpStatusCode.OK);
        }
    }

    public class UserLogInCommandWithSessionData
    {
        public Guid Id { get; set; }
        public EntityStatusType Status { get; set; }
        public UserStatusType UserStatus { get; set; }
    }
}
