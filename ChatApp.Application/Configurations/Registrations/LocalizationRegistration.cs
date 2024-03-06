using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace ChatApp.Application.Configurations.Registrations
{
    public static class LocalizationRegistration
    {
        public static void RegisterLocalization(this WebApplication app, IConfiguration configuration)
        {
            List<CultureInfo> supportedCultures = new();
            var languages = configuration.GetSection("LocalizationSettings:SupportedLanguageCodes").Value?.Split(',') ?? Array.Empty<string>();
            foreach (var language in languages)
                supportedCultures.Add(new CultureInfo(language));

            var localizationOptions = new RequestLocalizationOptions
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            localizationOptions.SetDefaultCulture(configuration.GetSection("LocalizationSettings:DefaultCultureCode").Value ?? languages.FirstOrDefault());
            localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

            app.UseRequestLocalization(localizationOptions);
        }
    }
}
