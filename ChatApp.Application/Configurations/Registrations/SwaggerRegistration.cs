using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using System.Linq;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ChatApp.Application.Configurations.Registrations
{
    public static class SwaggerRegistration
    {
        public static void RegisterSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = $"{Assembly.GetEntryAssembly()?.GetName().Name?.Split('.')[0]} API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = $"A ChatApp project by Baris Celebi",
                        Contact = new OpenApiContact
                        {
                            Name = "Baris Celebi",
                            Url = new Uri("https://barisclb.com")
                        }
                    });
                }

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

                options.OperationFilter<AddRequiredHeaderParameter>();
            });
        }

        public static void RegisterSwaggerUI(this WebApplication app, IWebHostEnvironment environment)
        {
            //if (!environment.IsDevelopment()) // TODO: revert
            //    return;

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        $"{Assembly.GetEntryAssembly()?.GetName().Name?.Split('.')[0]} {description.GroupName.ToUpperInvariant()}");
                }
            });
        }

        private class AddRequiredHeaderParameter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                operation.Parameters ??= new List<OpenApiParameter>();
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "x-api-key",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
            }
        }
    }
}
