using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Helpers;
using ChatApp.Application.Interfaces.Repositories.Mongo;
using ChatApp.Application.Interfaces.Repositories.Sql.Dapper;
using ChatApp.Application.Interfaces.Repositories.Sql.Efc;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Settings;
using ChatApp.Persistence.Contexts;
using ChatApp.Persistence.Repositories.Mongo;
using ChatApp.Persistence.Repositories.Redis;
using ChatApp.Persistence.Repositories.Sql.Dapper;
using ChatApp.Persistence.Repositories.Sql.Efc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ChatApp.Persistence
{
    public static class ServiceRegistration
    {
        public static void RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            registerSqlDatabase(services, configuration);

            services.AddScoped<IUserSqlEfcRepository, UserSqlEfcRepository>();
            services.AddScoped<IUserRoleSqlEfcRepository, UserRoleSqlEfcRepository>();
            services.AddScoped<IUserTokenSqlEfcRepository, UserTokenSqlEfcRepository>();
            services.AddScoped<ILanguageSqlEfcRepository, LanguageSqlEfcRepository>();
            services.AddScoped<IEmailVerificationSqlEfcRepository, EmailVerificationSqlEfcRepository>();

            services.AddScoped<IEmailArchiveMongoRepository, EmailArchiveMongoRepository>();
            services.AddScoped<IChatArchiveMongoRepository, ChatArchiveMongoRepository>();

            services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis") ?? throw new Exception("ConnectionString was not provided for ConnectionMultiplexer Injection.")));
            services.AddScoped<ICacheService, RedisRepository>();
        }

        public static async Task RegisterPersistenceApps(this WebApplication app)
        {
            using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ChatAppDbContext>();
            RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)dbContext.Database.GetService<IDatabaseCreator>();
            dbContext.Database.EnsureCreated();

            var databaseInitialization = serviceScope.ServiceProvider.GetRequiredService<IDatabaseInitialization>();
            await databaseInitialization.SeedSqlDatabase();
        }

        private static void registerSqlDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var sqlDbChoice = configuration.GetSection("ProjectSettings:SqlDbChoice").Value;

            if (sqlDbChoice == SqlDbChoiceType.MySql.ToString())
            {
                services.AddDbContext<ChatAppDbContext>(options => options.UseMySql(configuration.GetConnectionString("MySql"), ServerVersion.AutoDetect(configuration.GetConnectionString("MySql"))));
                services.AddScoped<IBaseSqlDapperRepository, MySqlDapperRepository>();
                SqlQueryHelper.SqlDatabaseChoice = SqlDbChoiceType.MySql;
            }
            else if (sqlDbChoice == SqlDbChoiceType.PostgreSql.ToString())
            {
                services.AddDbContext<ChatAppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgreSql")));
                services.AddScoped<IBaseSqlDapperRepository, PostgreSqlDapperRepository>();
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                SqlQueryHelper.SqlDatabaseChoice = SqlDbChoiceType.PostgreSql;
            }
            else if (sqlDbChoice == SqlDbChoiceType.MsSql.ToString())
            {
                services.AddDbContext<ChatAppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MsSql")));
                services.AddScoped<IBaseSqlDapperRepository, MsSqlDapperRepository>();
                SqlQueryHelper.SqlDatabaseChoice = SqlDbChoiceType.MsSql;
            }
            else
                throw new Exception("Unsupported Sql Database.");
        }
    }
}
