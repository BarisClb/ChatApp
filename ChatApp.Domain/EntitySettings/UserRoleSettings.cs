using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Domain.EntitySettings
{
    public class UserRoleSettings : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole").HasKey(userRole => userRole.Id);

            builder.Property(userRole => userRole.IsAdmin).IsRequired().HasDefaultValue(false);

            builder.Property(userRole => userRole.DateCreated).IsRequired();
            builder.Property(userRole => userRole.DateUpdated);
            builder.HasIndex(userRole => userRole.Status);

            builder.HasOne(userRole => userRole.User).WithMany(user => user.UserRole).HasForeignKey(userRole => userRole.UserId).HasPrincipalKey(user => user.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
