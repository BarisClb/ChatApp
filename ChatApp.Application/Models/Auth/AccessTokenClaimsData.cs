namespace ChatApp.Application.Models.Auth
{
    public class AccessTokenClaimsData
    {
        public string AccessTokenId { get; set; }
        public string UserId { get; set; }
        public string UserStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string LanguageCode { get; set; }
        public string UserDateCreated { get; set; }
        public string IsAdmin { get; set; }
    }
}
