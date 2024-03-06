namespace ChatApp.Application.Models.Settings
{
    public class MongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string EmailArchiveCollectionName { get; set; }
        public string ChatArchiveCollectionName { get; set; }
    }
}
