using ChatApp.Application.Models.Auth;
using ChatApp.Application.Models.Responses;
using ChatApp.Application.Models.Responses.Common;

namespace ChatApp.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<GenerateTokenData> GenerateTokens(Guid userId, Guid? refreshTokenId = default, GenerateTokenUserInfo? userInfo = default, GenerateTokenUserRoleInfo? userRoleInfo = default);
        Task<GenerateAccessTokenData> GenerateAccessToken(Guid userId, GenerateTokenUserInfo? userInfo = default, GenerateTokenUserRoleInfo? userRoleInfo = default, DateTime? tokenExpirationDate = default);
        Task<GenerateRefreshTokenData> GenerateRefreshToken(Guid userId, Guid? refreshTokenId, DateTime? tokenExpirationDate = default);
        Task<RefreshTokenClaimsData> GetRefreshTokenClaims(string refreshToken);
        Task<AccessTokenClaimsData> GetAccessTokenClaims(string accessToken);
        Task<ApiResponse<UserSessionResponse>> RefreshAccessToken();
    }
}
