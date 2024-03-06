using ChatApp.Application.Helpers;
using ChatApp.Application.Interfaces.Helpers;
using ChatApp.Application.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ChatApp.Application.Configurations.Middlewares
{
    public class LocalizationHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILanguageHelper _languageHelper;
        private readonly IOptions<LocalizationSettings> _localizationSettings;

        public LocalizationHandlerMiddleware(RequestDelegate next, ILanguageHelper languageHelper, IOptions<LocalizationSettings> localizationSettings)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _languageHelper = languageHelper ?? throw new ArgumentNullException(nameof(languageHelper));
            _localizationSettings = localizationSettings;
        }


        public async Task Invoke(HttpContext context)
        {
            await RegisterLocalization(context);
            await _next(context);
        }

        private async Task RegisterLocalization(HttpContext httpContext)
        {
            var headerCultureCode = httpContext.Request.GetHeaderValue("Accept-Language");
            var cultureCode = _languageHelper.GetLanguages.FirstOrDefault(language => language.Code == headerCultureCode)?.Code;
            if (cultureCode == null)
            {
                cultureCode = _localizationSettings.Value.DefaultCultureCode;
                httpContext.Request.Headers["Accept-Language"] = cultureCode;
                await LocalizationHelper.ChangeCurrentCulture(cultureCode);
            }
            else if (cultureCode != _localizationSettings.Value.DefaultCultureCode)
            {
                httpContext.Request.Headers["Accept-Language"] = cultureCode;
                await LocalizationHelper.ChangeCurrentCulture(cultureCode);
            }
        }
    }
}
