using ChatApp.Application.Interfaces.Helpers;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Data;
using ChatApp.Domain.Enums;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ChatApp.Application.Helpers
{
    public class LanguageHelperMsSql : ILanguageHelper
    {
        public LanguageHelperMsSql(IConfiguration configuration, ICacheService cacheService)
        {
            ConnectionString = configuration.GetConnectionString("MsSql") ?? throw new Exception("ConnectionString was not provided for LanguageHelperMsSql.");
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            UseCache = bool.Parse(configuration.GetSection("ProjectSettings:UseCacheForLanguageHelper").Value ?? "true");
        }

        private readonly ICacheService _cacheService;
        private string ConnectionString { get; set; }
        private bool UseCache { get; set; }
        private List<LanguageHelperLanguage> languages { get; set; } = new();

        // If we want to get the data from Database during first initialization and keep using the same Data, we can Register this class as Singleton and only make a Database call at first initialization
        private List<LanguageHelperLanguage> LanguagesFromField
        {
            get
            {
                if (languages == null || !languages.Any())
                {
                    using var connection = new SqlConnection(ConnectionString);
                    connection.Open();
                    var languageList = connection.Query<LanguageHelperLanguage>(SqlQueryHelper.GetLanguagesForLanguageHelperQuery, new { EntityStatusTypeActive = (int)EntityStatusType.Active });
                    languages = (languageList != null && languageList.Any()) ? languageList.ToList() : new();
                }
                return languages;
            }
        }

        // We can also use Cache, this way we can refresh the list without having to Restart the Project
        private List<LanguageHelperLanguage> LanguagesFromCache
        {
            get
            {
                var cachedLanguages = _cacheService.GetValue<List<LanguageHelperLanguage>>(CacheHelper.LanguageHelperLanguagesKey);
                if (cachedLanguages == null || !cachedLanguages.Any())
                {
                    using var connection = new SqlConnection(ConnectionString);
                    connection.Open();
                    var languages = connection.Query<LanguageHelperLanguage>(SqlQueryHelper.GetLanguagesForLanguageHelperQuery, new { EntityStatusTypeActive = (int)EntityStatusType.Active }).ToList();
                    if (languages != null)
                    {
                        _cacheService.SetValue(CacheHelper.LanguageHelperLanguagesKey, languages, CacheHelper.LanguageHelperLanguagesExpirationTime);
                        return languages;
                    }
                    return new();
                }
                else
                    return cachedLanguages;
            }
        }

        public List<LanguageHelperLanguage> GetLanguages
        {
            get
            {
                return UseCache ? LanguagesFromCache : LanguagesFromField;
            }
        }
    }
}
