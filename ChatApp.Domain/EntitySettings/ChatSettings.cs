using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Domain.EntitySettings
{
    public class ChatSettings : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("Chat").HasKey(chat => chat.Id);

            builder.Property(chat => chat.Name).IsRequired();
            builder.Property(chat => chat.ImageBase64);

            builder.Property(chat => chat.DateCreated).IsRequired();
            builder.Property(chat => chat.DateUpdated);
            builder.HasIndex(chat => chat.Status);
        }
    }
}
