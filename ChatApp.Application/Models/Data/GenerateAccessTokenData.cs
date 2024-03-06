using ChatApp.Application.Models.Auth;

namespace ChatApp.Application.Models.Data
{
    public class GenerateAccessTokenData
    {
        public string AccessToken { get; set; }
        public AccessTokenClaimsData AccessTokenClaimsData { get; set; }
    }
}
