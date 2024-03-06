using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Domain.EntitySettings
{
    public class UserSettings : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User").HasKey(user => user.Id);

            builder.Property(user => user.FirstName).IsRequired();
            builder.Property(user => user.LastName).IsRequired();
            builder.HasIndex(user => user.EmailAddress).IsUnique();
            builder.HasIndex(user => user.Username).IsUnique();
            builder.HasIndex(user => user.Password);
            builder.Property(user => user.BirthDay);
            builder.HasIndex(user => user.UserStatus);

            builder.Property(user => user.DateCreated).IsRequired();
            builder.Property(user => user.DateUpdated);
            builder.HasIndex(user => user.Status);

            builder.HasOne(user => user.Language).WithMany(language => language.User).HasForeignKey(user => user.LanguageId).HasPrincipalKey(language => language.Id).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(user => user.UserInvited).WithOne(userInvitation => userInvitation.InvitedUser).HasForeignKey(userInvitation => userInvitation.InvitedUserId).HasPrincipalKey(user => user.Id).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(user => user.UserInviter).WithOne(userInvitation => userInvitation.InviterUser).HasForeignKey(userInvitation => userInvitation.InviterUserId).HasPrincipalKey(user => user.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
