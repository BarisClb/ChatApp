using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Domain.EntitySettings
{
    public class EmailVerificationSettings : IEntityTypeConfiguration<EmailVerification>
    {
        public void Configure(EntityTypeBuilder<EmailVerification> builder)
        {
            builder.ToTable("EmailVerification").HasKey(emailVerification => emailVerification.Id);

            builder.Property(emailVerification => emailVerification.EmailVerificationType).IsRequired();
            builder.Property(emailVerification => emailVerification.VerificationCode).IsRequired();
            builder.Property(emailVerification => emailVerification.VerificationId).IsRequired();

            builder.Property(emailVerification => emailVerification.DateCreated).IsRequired();
            builder.Property(emailVerification => emailVerification.DateUpdated);
            builder.HasIndex(emailVerification => emailVerification.Status);

            builder.HasOne(emailVerification => emailVerification.User).WithMany(user => user.EmailVerification).HasForeignKey(emailVerification => emailVerification.UserId).HasPrincipalKey(user => user.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
