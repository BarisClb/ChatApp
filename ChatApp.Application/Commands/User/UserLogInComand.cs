using AutoMapper;
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
    public class UserLogInCommand : IRequest<ApiResponse<UserTokenResponse>>
    {
        public string UserField { get; set; }
        public string Password { get; set; }
    }

    public class UserLogInCommandHandler : IRequestHandler<UserLogInCommand, ApiResponse<UserTokenResponse>>
    {
        private readonly IBaseSqlDapperRepository _sqlDapperRepository;
        private readonly ITokenService _tokenService;
        private readonly IStringLocalizer<LocalizationResources> _localizer;

        public UserLogInCommandHandler(IBaseSqlDapperRepository baseSqlDapperRepository, ITokenService tokenService, IStringLocalizer<LocalizationResources> localizer)
        {
            _sqlDapperRepository = baseSqlDapperRepository ?? throw new ArgumentNullException(nameof(baseSqlDapperRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }


        public async Task<ApiResponse<UserTokenResponse>> Handle(UserLogInCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserField))
                return ApiResponse<UserTokenResponse>.Fail(_localizer[LocalizationKeys.UserLogInUserFieldCantBeEmpty], (int)HttpStatusCode.BadRequest);
            if (string.IsNullOrEmpty(request.Password))
                return ApiResponse<UserTokenResponse>.Fail(string.Format(_localizer[LocalizationKeys.FollowingFieldWasNotProvided], _localizer[LocalizationKeys.Password]), (int)HttpStatusCode.BadRequest);

            string userFieldQuery = ValidationHelper.CheckIfStringIsInEmailFormat(request.UserField) ? "EmailAddress" : "Username";

            var user = await _sqlDapperRepository.QueryFirstOrDefaultAsync<UserLogInCommandData>(string.Format(SqlQueryHelper.UserLogInCommandQuery, userFieldQuery), new { UserField = request.UserField, Password = SecurityHelper.GetSha256Hash(request.Password) });

            if (user == null)
                return ApiResponse<UserTokenResponse>.Fail(_localizer[LocalizationKeys.UserLogInUserWasNotFound], (int)HttpStatusCode.BadRequest);
            if (user.UserStatus == UserStatusType.Inactive)
                return ApiResponse<UserTokenResponse>.Fail(_localizer[LocalizationKeys.UserLogInUserIsInactive], (int)HttpStatusCode.BadRequest);

            var tokens = await _tokenService.GenerateTokens(user.Id);

            return ApiResponse<UserTokenResponse>.Success(new() { AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken }, (int)HttpStatusCode.OK);
        }
    }

    public class UserLogInCommandData
    {
        public Guid Id { get; set; }
        public EntityStatusType Status { get; set; }
        public UserStatusType UserStatus { get; set; }
    }
}
