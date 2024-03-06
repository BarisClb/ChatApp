using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Domain.EntitySettings
{
    public class LanguageSettings : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Language").HasKey(language => language.Id);

            builder.Property(language => language.Name).IsRequired();
            builder.Property(language => language.Code).IsRequired();

            builder.Property(language => language.DateCreated);
            builder.Property(language => language.DateUpdated);
            builder.HasIndex(language => language.Status);
        }
    }
}
