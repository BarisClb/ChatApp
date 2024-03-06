using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Domain.EntitySettings
{
    public class MessageSettings : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message").HasKey(message => message.Id);

            builder.Property(message => message.Text).IsRequired();

            builder.Property(message => message.DateCreated);
            builder.Property(message => message.DateUpdated);
            builder.HasIndex(message => message.Status);

            builder.HasOne(message => message.User).WithMany(user => user.Message).HasForeignKey(message => message.UserId).HasPrincipalKey(user => user.Id).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(message => message.Chat).WithMany(chat => chat.Message).HasForeignKey(message => message.ChatId).HasPrincipalKey(chat => chat.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
