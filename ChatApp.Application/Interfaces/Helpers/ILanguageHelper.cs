using ChatApp.Application.Models.Data;

namespace ChatApp.Application.Interfaces.Helpers
{
    public interface ILanguageHelper
    {
        List<LanguageHelperLanguage> GetLanguages { get; }
    }
}
