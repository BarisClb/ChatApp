using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Domain.EntitySettings
{
    public class UserInvitationSettings : IEntityTypeConfiguration<UserInvitation>
    {
        public void Configure(EntityTypeBuilder<UserInvitation> builder)
        {
            builder.ToTable("UserInvitation").HasKey(userInvitation => userInvitation.Id);

            builder.Property(userInvitation => userInvitation.UserInvitationType).IsRequired();
            builder.Property(userInvitation => userInvitation.InvitationReferenceId).IsRequired();

            builder.Property(userInvitation => userInvitation.DateCreated).IsRequired();
            builder.Property(userInvitation => userInvitation.DateUpdated);
            builder.HasIndex(userInvitation => userInvitation.Status);

            builder.HasOne(userInvitation => userInvitation.InviterUser).WithMany(user => user.UserInviter).HasForeignKey(userInvitation => userInvitation.InviterUserId).HasPrincipalKey(user => user.Id).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(userInvitation => userInvitation.InvitedUser).WithMany(user => user.UserInvited).HasForeignKey(userInvitation => userInvitation.InvitedUserId).HasPrincipalKey(user => user.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
