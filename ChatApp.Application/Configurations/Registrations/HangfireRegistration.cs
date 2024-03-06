using ChatApp.Application.Models.Settings;
using Hangfire;
using Hangfire.MySql;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Application.Configurations.Registrations
{
    public static class HangfireRegistration
    {
        public static void RegisterHangfireService(this IServiceCollection services, IConfiguration configuration)
        {
            string? hangfireConnectionString = configuration.GetConnectionString(configuration.GetSection("ProjectSettings:SqlDbChoice").Value ?? throw new Exception("ConnectionDatabase was not provided for RegisterHangfireService.")) ?? throw new Exception("ConnectionString was not provided for RegisterHangfireService.");
            string? schemaName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name?.Split('.')[0] ?? throw new Exception("ProjectName was not provided for RegisterHangfireService.");
            var sqlDbChoice = configuration.GetSection("ProjectSettings:SqlDbChoice").Value;

            if (sqlDbChoice == SqlDbChoiceType.MySql.ToString())
            {
                services.AddHangfire(configuration => configuration.UseSimpleAssemblyNameTypeSerializer()
                                                                   .UseRecommendedSerializerSettings()
                                                                   .UseStorage(new MySqlStorage(hangfireConnectionString, new MySqlStorageOptions
                                                                   {
                                                                       QueuePollInterval = TimeSpan.FromSeconds(10),
                                                                       JobExpirationCheckInterval = TimeSpan.FromHours(1),
                                                                       CountersAggregateInterval = TimeSpan.FromMinutes(5),
                                                                       TablesPrefix = "Hangfire"
                                                                   })));
            }
            else if (sqlDbChoice == SqlDbChoiceType.PostgreSql.ToString())
            {
                services.AddHangfire(configuration => configuration.UseSimpleAssemblyNameTypeSerializer()
                                                                   .UseRecommendedSerializerSettings()
                                                                   .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(hangfireConnectionString), new PostgreSqlStorageOptions()
                                                                   {
                                                                       TransactionSynchronisationTimeout = TimeSpan.FromMinutes(5),
                                                                       InvisibilityTimeout = TimeSpan.FromMinutes(5),
                                                                       QueuePollInterval = TimeSpan.FromSeconds(10),
                                                                       SchemaName = schemaName + "Jobs"
                                                                   }));
            }
            else if (sqlDbChoice == SqlDbChoiceType.MsSql.ToString())
            {
                services.AddHangfire(configuration => configuration.UseSimpleAssemblyNameTypeSerializer()
                                                                   .UseRecommendedSerializerSettings()
                                                                   .UseSqlServerStorage(hangfireConnectionString, new SqlServerStorageOptions
                                                                   {
                                                                       CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                                                       SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                                                       QueuePollInterval = TimeSpan.FromSeconds(10),
                                                                       UseRecommendedIsolationLevel = true,
                                                                       DisableGlobalLocks = true,
                                                                       SchemaName = schemaName + "Jobs",
                                                                   }));
            }
            else
                throw new Exception("Unsupported Sql Database.");

            services.AddHangfireServer();
        }

        public static void RegisterHangfireDashboard(this WebApplication app, IConfiguration configuration)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] {
                    new HangfireCustomBasicAuthenticationFilter()
                    {
                        User = configuration.GetSection("HangfireSettings:Username").Value,
                        Pass = configuration.GetSection("HangfireSettings:Password").Value
                    }
                }
            });
        }

        public static void RegisterHangfireJobs(this WebApplication app, IConfiguration configuration)
        {
            //Hangfire.RecurringJob.AddOrUpdate<IJobService>(job => job.DisablePendingAccounts(), configuration["HangfireSettings:DisablePendingAccountsJobCronTime"]);
        }
    }
}
