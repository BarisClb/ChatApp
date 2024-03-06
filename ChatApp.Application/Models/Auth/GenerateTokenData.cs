namespace ChatApp.Application.Models.Auth
{
    public class GenerateTokenData
    {
        public AccessTokenClaimsData AccessTokenClaims { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
