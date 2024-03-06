using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Domain.EntitySettings
{
    public class UserTokenSettings : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserToken").HasKey(userRole => userRole.Id);

            builder.HasIndex(userToken => userToken.RefreshTokenId);
            builder.Property(userToken => userToken.IssueDate).IsRequired();
            builder.Property(userToken => userToken.ExpirationDate).IsRequired();

            builder.Property(userToken => userToken.DateCreated).IsRequired();
            builder.Property(userToken => userToken.DateUpdated);
            builder.Property(userToken => userToken.Status).IsRequired();

            builder.HasOne(userToken => userToken.User).WithMany(user => user.UserToken).HasForeignKey(userToken => userToken.UserId).HasPrincipalKey(user => user.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
