using ChatApp.Domain.Entities.Common;

namespace ChatApp.Domain.Entities
{
    public class Message : BaseEntity
    {
        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Text { get; set; }
    }
}
