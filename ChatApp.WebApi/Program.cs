using ChatApp.Application;
using ChatApp.Infrastructure;
using ChatApp.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, true)
                     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, true)
                     .AddEnvironmentVariables()
                     .Build();
builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = null; });
builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.RegisterApplicationServices(builder.Configuration, builder.Host);
builder.Services.RegisterInfrastructureServices();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
await app.RegisterPersistenceApps();
app.RegisterApplicationApps(builder.Configuration);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
