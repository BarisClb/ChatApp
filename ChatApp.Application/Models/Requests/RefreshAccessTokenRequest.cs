namespace ChatApp.Application.Models.Requests
{
    public class RefreshAccessTokenRequest
    {
        public Guid RefreshTokenId { get; set; }
        public string RefreshToken { get; set; }
    }
}
