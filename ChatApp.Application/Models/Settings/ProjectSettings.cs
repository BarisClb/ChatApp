namespace ChatApp.Application.Models.Settings
{
    public class ProjectSettings
    {
        public string SqlDbChoice { get; set; }
        public bool IsVerificationRequired { get; set; }
        public bool UseCacheForLanguageHelper { get; set; }
    }

    public enum SqlDbChoiceType
    {
        MySql = 1,
        PostgreSql = 2,
        MsSql = 3
    }
}
