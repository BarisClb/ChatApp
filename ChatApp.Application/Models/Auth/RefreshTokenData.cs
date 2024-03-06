namespace ChatApp.Application.Models.Auth
{
    public class RefreshTokenData
    {
        public Guid? UserId { get; set; }
        public Guid? RefreshTokenId { get; set; }
    }
}
