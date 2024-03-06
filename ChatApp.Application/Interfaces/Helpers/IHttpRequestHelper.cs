using ChatApp.Application.Models.Auth;

namespace ChatApp.Application.Interfaces.Helpers
{
    public interface IHttpRequestHelper
    {
        Task<CurrentUser> GetCurrentUser();
        Task<CurrentUserWithHeaders> GetCurrentUserWithHeaders();
        Task<AccessTokenData> GeAccessTokenClaimsData();
        Task<RefreshTokenData> GetRefreshTokenClaimsData();
        Task<GetTokensFromHeaderData> GetHeaders();
    }
}
