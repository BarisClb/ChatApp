using ChatApp.WebHub.Configurations;
using ChatApp.WebHub.Services;
#if !DEBUG
using Microsoft.AspNetCore.Server.Kestrel.Core;
#endif

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

#if !DEBUG
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
    serverOptions.ListenAnyIP(8081, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
#endif

builder.Services.AddCors(options => options.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .SetIsOriginAllowed(host => true)
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapGrpcService<GrpcService>();
app.MapHub<HubService>("/chatapphub");
app.MapControllers();

app.Run();