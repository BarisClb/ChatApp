namespace ChatApp.Application.Models.Responses
{
    public class UserTokenResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
