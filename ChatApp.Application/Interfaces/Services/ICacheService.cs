namespace ChatApp.Application.Interfaces.Services
{
    public interface ICacheService
    {
        Task<T?> GetValueAsync<T>(string key);
        Task<bool> DeleteValueAsync(string key);
        Task<bool> SetValueAsync<T>(string key, T value, TimeSpan? expirationTime);
        Task<bool> SetValueUnderFolderAsync<T>(string key, string folderName, T value, TimeSpan? expirationTime);
        Task DeleteByPatternAsync(string pattern);

        T? GetValue<T>(string key);
        bool DeleteValue(string key);
        bool SetValue<T>(string key, T value, TimeSpan? expirationTime);
        bool SetValueUnderFolder<T>(string key, string folderName, T value, TimeSpan? expirationTime);
        void DeleteByPattern(string pattern);
    }
}
