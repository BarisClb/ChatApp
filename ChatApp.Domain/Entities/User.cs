using ChatApp.Domain.Entities.Common;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities
{
    public class User : BaseEntity
    {
        public new Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDay { get; set; }
        public UserStatusType UserStatus { get; set; }

        public int LanguageId { get; set; }
        public Language Language { get; set; }

        public ICollection<EmailVerification>? EmailVerification { get; set; }
        public ICollection<Message>? Message { get; set; }
        public ICollection<ChatUser>? ChatUser { get; set; }
        public ICollection<UserRole>? UserRole { get; set; }
        public ICollection<UserToken>? UserToken { get; set; }
        public ICollection<UserInvitation>? UserInvited { get; set; }
        public ICollection<UserInvitation>? UserInviter { get; set; }
    }
}
