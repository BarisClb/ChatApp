namespace ChatApp.Application.Helpers
{
    public static class CacheHelper
    {
        public static string AccessTokenIdKey => "UserToken:AccessTokenId:userId_{0}";
        public static TimeSpan? AccessTokenIdExpirationTime => TimeSpan.FromDays(7);

        public static string UserByIdKey => "User:UserById:userId_{0}";
        public static TimeSpan? UserByIdExpirationTime => TimeSpan.FromHours(1);

        public static string UserByUsernameKey => "User:UserByUsername:userUsername_{0}";
        public static TimeSpan? UserByUsernameExpirationTime => TimeSpan.FromHours(1);

        public static string LanguageHelperLanguagesKey => "Language:languageHelper";
        public static TimeSpan? LanguageHelperLanguagesExpirationTime => TimeSpan.FromDays(1);
    }
}
