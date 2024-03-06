using ChatApp.Domain.Entities.Common;

namespace ChatApp.Domain.Entities
{
    public class UserToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid RefreshTokenId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
