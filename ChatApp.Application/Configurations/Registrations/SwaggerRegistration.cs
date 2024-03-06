using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ChatApp.Application.Configurations.Registrations
{
    public static class SwaggerRegistration
    {
        public static void RegisterSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name?.Split('.')[0], Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Description = "Authorization header using the Bearer scheme. Bearer {token}"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static void RegisterSwaggerUI(this WebApplication app, IWebHostEnvironment environment)
        {
            if (true || environment.IsDevelopment()) // TODO: REVERT
            {
                app.UseSwagger();
                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("v1/swagger.json", System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name?.Split('.')[0]);
                });
            }
        }
    }
}
