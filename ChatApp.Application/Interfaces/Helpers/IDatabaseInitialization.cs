namespace ChatApp.Application.Interfaces.Helpers
{
    public interface IDatabaseInitialization
    {
        Task SeedSqlDatabase();
    }
}
