namespace ChatApp.Application.Models.Responses
{
    public class UserSessionResponse
    {
        public Guid? UserId { get; set; }
        public int? UserStatus { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? Username { get; set; }
        public string? LanguageCode { get; set; }
        public DateTime? UserDateCreated { get; set; }
        public bool? IsAdmin { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
