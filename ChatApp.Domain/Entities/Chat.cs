using ChatApp.Domain.Entities.Common;

namespace ChatApp.Domain.Entities
{
    public class Chat : BaseEntity
    {
        public string Name { get; set; }
        public string? ImageBase64 { get; set; }

        public ICollection<Message>? Message { get; set; }
        public ICollection<ChatUser>? ChatUser { get; set; }
        public ICollection<UserInvitation>? UserInvitation { get; set; }
    }
}
