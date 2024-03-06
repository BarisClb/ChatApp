using ChatApp.Domain.Enums;

namespace ChatApp.Application.Models.Auth
{
    public class CurrentUser
    {
        public Guid? UserId { get; set; }
        public UserStatusType? UserStatus { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? Username { get; set; }
        public string? LanguageCode { get; set; }
        public DateTime? UserDateCreated { get; set; }
        public bool? IsAdmin { get; set; }
    }

    public class CurrentUserWithHeaders : CurrentUser
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public Guid? RefreshTokenId { get; set; }
    }
}
