using ChatApp.Application.Configurations.Pipelines;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Infrastructure.Email;
using ChatApp.Infrastructure.Jwt;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChatApp.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(executingAssembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddAutoMapper(executingAssembly);

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, JwtService>();
        }
    }
}
