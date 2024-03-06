using System.Globalization;
using System.Resources;

namespace ChatApp.Application.Helpers
{
    public static class LocalizationHelper
    {
        private static ResourceManager _resourceManager;

        static LocalizationHelper()
        {
            _resourceManager = new ResourceManager($"{System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name}.Resources.LocalizationResources", System.Reflection.Assembly.GetExecutingAssembly());
        }


        public static async Task ChangeCurrentCulture(string cultureCode)
        {
            var cultureInfo = new CultureInfo(cultureCode);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }

        public static async Task<string> GetString(string key) // TODO: TEST THIS - we might not need the middleware either
        {
            return _resourceManager?.GetString(key) ?? "";
        }
    }
}
