using ChatApp.Domain.Enums;

namespace ChatApp.Application.Models.Auth
{
    public class GenerateTokenUserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string LanguageCode { get; set; }
        public UserStatusType UserStatus { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
