using ChatApp.Domain.Entities.Common;

namespace ChatApp.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public bool IsAdmin { get; set; }
    }
}
