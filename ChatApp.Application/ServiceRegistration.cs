using Asp.Versioning;
using ChatApp.Application.Configurations.Database;
using ChatApp.Application.Configurations.Middlewares;
using ChatApp.Application.Configurations.Registrations;
using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Helpers;
using ChatApp.Application.Models.Auth;
using ChatApp.Application.Models.Settings;
using ChatApp.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace ChatApp.Application
{
    public static class ServiceRegistration
    {
        public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration, ConfigureHostBuilder host)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("x-api-version"),
                    new UrlSegmentApiVersionReader());
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<EmailServiceSettings>(configuration.GetSection("EmailServiceSettings"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<LocalizationSettings>(configuration.GetSection("LocalizationSettings"));
            services.Configure<ProjectSettings>(configuration.GetSection("ProjectSettings"));
            services.Configure<SqlDatabaseSeedingSettings>(configuration.GetSection("SqlDatabaseSeedingSettings"));

            services.AddMediatR(conf => conf.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddScoped<IDatabaseInitialization, DatabaseInitialization>();
            services.AddScoped<IHttpRequestHelper, HttpRequestHelper>();

            services.AddScoped<CurrentUser>();
            services.AddScoped<CurrentUserWithHeaders>();
            services.AddScoped<TestService>();
            services.registerLanguageHelper(configuration);
            services.registerCorsService();

            services.RegisterJwtToken(configuration);
            services.RegisterSwaggerGen();
            services.RegisterHangfireService(configuration);
            host.UseSerilog(ElasticsearchRegistration.Configure);

            services.AddHttpContextAccessor();
            services.AddLocalization();
        }

        public static void RegisterApplicationApps(this WebApplication app, IConfiguration configuration)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<LocalizationHandlerMiddleware>();
            app.UseMiddleware<IdentityHandlerMiddleware>();

            app.registerCorsApp();
            app.RegisterSwaggerUI(app.Environment);
            app.RegisterHangfireDashboard(configuration);
            app.RegisterHangfireJobs(configuration);
            app.RegisterLocalization(configuration);
        }

        private static void registerLanguageHelper(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlDbChoice = configuration.GetSection("ProjectSettings:SqlDbChoice").Value;
            bool useCacheForLanguageHelper = !string.Equals(configuration.GetSection("ProjectSettings:UseCacheForLanguageHelper").Value, "false", StringComparison.OrdinalIgnoreCase);
            if (sqlDbChoice == SqlDbChoiceType.MySql.ToString())
            {
                if (useCacheForLanguageHelper)
                    services.AddScoped<ILanguageHelper, LanguageHelperMySql>();
                else
                    services.AddSingleton<ILanguageHelper, LanguageHelperMySql>();
            }
            else if (sqlDbChoice == SqlDbChoiceType.PostgreSql.ToString())
            {
                if (useCacheForLanguageHelper)
                    services.AddScoped<ILanguageHelper, LanguageHelperPostgreSql>();
                else
                    services.AddSingleton<ILanguageHelper, LanguageHelperPostgreSql>();
            }
            else if (sqlDbChoice == SqlDbChoiceType.MsSql.ToString())
            {
                if (useCacheForLanguageHelper)
                    services.AddScoped<ILanguageHelper, LanguageHelperMsSql>();
                else
                    services.AddSingleton<ILanguageHelper, LanguageHelperMsSql>();
            }
            else
                throw new Exception("Unsupported Sql Database.");
        }

        private static void registerCorsService(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                        builder =>
                        {
                            builder.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader();
                        });
            });
        }

        private static void registerCorsApp(this WebApplication app)
        {
            app.UseCors("AllowAll");
        }
    }
}
