using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Domain.EntitySettings
{
    public class ChatUserSettings : IEntityTypeConfiguration<ChatUser>
    {
        public void Configure(EntityTypeBuilder<ChatUser> builder)
        {
            builder.ToTable("ChatUser").HasKey(chatUser => chatUser.Id);

            builder.Property(chatUser => chatUser.DateCreated).IsRequired();
            builder.Property(chatUser => chatUser.DateUpdated);
            builder.HasIndex(chatUser => chatUser.Status);

            builder.HasOne(chatUser => chatUser.User).WithMany(user => user.ChatUser).HasForeignKey(chatUser => chatUser.UserId).HasPrincipalKey(user => user.Id).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(chatUser => chatUser.Chat).WithMany(chat => chat.ChatUser).HasForeignKey(chatUser => chatUser.ChatId).HasPrincipalKey(chat => chat.Id).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
