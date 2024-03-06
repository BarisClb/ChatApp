namespace ChatApp.Application.Models.Auth
{
    public class GetTokensFromHeaderData
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
