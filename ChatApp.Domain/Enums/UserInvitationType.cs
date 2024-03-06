using System.ComponentModel;

namespace ChatApp.Domain.Enums
{
    public enum UserInvitationType
    {
        [Description("Friend Invitation")]
        FriendInvitation = 1,
        [Description("Chat Invitation")]
        ChatInvitation = 2
    }
}
