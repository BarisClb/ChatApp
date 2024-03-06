namespace ChatApp.Application.Configurations.Pipelines
{
    public interface ICacheableQuery
    {
        string CacheKey { get; }
        TimeSpan? ExpirationTime { get; }
    }
}
