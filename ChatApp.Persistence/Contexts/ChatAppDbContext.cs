using ChatApp.Domain.Entities;
using ChatApp.Domain.Entities.Common;
using ChatApp.Domain.EntitySettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChatApp.Persistence.Contexts
{
    public class ChatAppDbContext : DbContext
    {
        public ChatAppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Chat> Chat { get; set; }
        public DbSet<EmailVerification> EmailVerification { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<ChatUser> ChatUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserSettings).Assembly);
        }

        public override int SaveChanges()
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                if (data.State == EntityState.Added)
                    data.Entity.DateCreated = DateTime.UtcNow;
                else if (data.State == EntityState.Modified)
                    data.Entity.DateUpdated = DateTime.UtcNow;
            }
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                if (data.State == EntityState.Added)
                    data.Entity.DateCreated = DateTime.UtcNow;
                else if (data.State == EntityState.Modified)
                    data.Entity.DateUpdated = DateTime.UtcNow;
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
