using ChatApp.Domain.Entities.Common;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities
{
    public class UserInvitation : BaseEntity
    {
        public UserInvitationType UserInvitationType { get; set; }
        public int? InvitationReferenceId { get; set; }

        public Guid InviterUserId { get; set; }
        public User InviterUser { get; set; }

        public Guid InvitedUserId { get; set; }
        public User InvitedUser { get; set; }
    }
}
