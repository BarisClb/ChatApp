namespace ChatApp.Application.Models.Settings
{
    public class SqlDatabaseSeedingSettings
    {
        public bool SeedDatabase { get; set; }
        public bool SeedLanguages { get; set; }
        public bool SeedAdmin { get; set; }
        public string AdminFirstName { get; set; }
        public string AdminLastName { get; set; }
        public string AdminUsername { get; set; }
        public string AdminEmailAddress { get; set; }
        public string AdminPassword { get; set; }
        public bool SeedUsers { get; set; }
        public int NumberOfUsersToSeed { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserUsername { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPassword { get; set; }
    }
}
