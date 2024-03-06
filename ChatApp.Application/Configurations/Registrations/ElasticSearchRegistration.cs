using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace ChatApp.Application.Configurations.Registrations
{
    public class ElasticsearchRegistration
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (context, configuration) =>
           {
               string? elasticUri = context.Configuration.GetConnectionString("Elasticsearch");
               string? elasticUsername = context.Configuration["ElasticsearchSettings:Username"];
               string? elasticPassword = context.Configuration["ElasticsearchSettings:Password"];
               if (!string.IsNullOrEmpty(elasticUri))
                   configuration.MinimumLevel.Error()
                                .Enrich.FromLogContext()
                                .Enrich.WithExceptionDetails()
                                .Enrich.FromLogContext()
                                .WriteTo.Debug()
                                .WriteTo.Console()
                                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                                {
                                    IndexFormat = $"{context.HostingEnvironment.ApplicationName.ToLower().Split('.')[0]}-{context.HostingEnvironment.EnvironmentName?.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
                                    CustomFormatter = new ElasticsearchJsonFormatter(renderMessage: true),
                                    AutoRegisterTemplate = true,
                                    ModifyConnectionSettings = (settings => settings.BasicAuthentication(elasticUsername, elasticPassword))
                                })
                                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName.Split('.')[0])
                                .ReadFrom.Configuration(context.Configuration);
           };
    }
}
