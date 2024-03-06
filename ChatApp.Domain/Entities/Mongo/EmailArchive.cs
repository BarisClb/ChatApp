namespace ChatApp.Domain.Entities.Mongo
{
    public class EmailArchive
    {
        public Guid _id { get; set; }
        public string EmailSentTo { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
