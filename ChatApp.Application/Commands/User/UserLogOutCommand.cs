using ChatApp.Application.Interfaces.Helpers;
using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Application.Models.Responses.Common;
using MediatR;
using System.Net;

namespace ChatApp.Application.Commands.User
{
    public class UserLogOutCommand : IRequest<ApiResponse<NoContent>>
    { }

    public class UserLogOutCommandHandler : IRequestHandler<UserLogOutCommand, ApiResponse<NoContent>>
    {
        private readonly IUserTokenSqlEfcRepository _userTokenSqlEfcRepository;
        private readonly IHttpRequestHelper _httpRequestHelper;

        public UserLogOutCommandHandler(IUserTokenSqlEfcRepository userTokenSqlEfcRepository, IHttpRequestHelper httpRequestHelper)
        {
            _userTokenSqlEfcRepository = userTokenSqlEfcRepository ?? throw new Exception(nameof(userTokenSqlEfcRepository));
            _httpRequestHelper = httpRequestHelper;
        }

        public async Task<ApiResponse<NoContent>> Handle(UserLogOutCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenData = await _httpRequestHelper.GetRefreshTokenClaimsData();

            if (refreshTokenData == null || refreshTokenData.UserId == null || refreshTokenData.RefreshTokenId == null || refreshTokenData.RefreshTokenId == Guid.Empty)
                return ApiResponse<NoContent>.Success((int)HttpStatusCode.OK);

            await _userTokenSqlEfcRepository.UserTokenDisableByUserAndRefreshTokenId(refreshTokenData.UserId ?? Guid.Empty, refreshTokenData.RefreshTokenId ?? Guid.Empty);

            return ApiResponse<NoContent>.Success((int)HttpStatusCode.OK);
        }
    }
}
