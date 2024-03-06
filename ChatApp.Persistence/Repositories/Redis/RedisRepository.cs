using ChatApp.Application.Interfaces.Services;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ChatApp.Persistence.Repositories.Redis
{
    public class RedisRepository : ICacheService
    {
        private readonly StackExchange.Redis.IDatabase _database;
        private readonly StackExchange.Redis.IServer[] _servers;
        private readonly string _projectName;

        public RedisRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase() ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            _servers = connectionMultiplexer.GetServers() ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            _projectName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name?.Split('.')[0] ?? throw new Exception("ProjectName was not provided for RedisRepository.");
        }


        public async Task<T?> GetValueAsync<T>(string key)
        {
            var data = await _database.StringGetAsync(key);
            if (data.HasValue)
                return JsonConvert.DeserializeObject<T>(data.ToString());
            return default;
        }

        public async Task<bool> DeleteValueAsync(string key)
        {
            var valueExists = await _database.KeyExistsAsync(key);
            if (valueExists)
                return await _database.KeyDeleteAsync(key);
            return false;
        }

        public async Task<bool> SetValueAsync<T>(string key, T value, TimeSpan? expirationTime)
        {
            return await _database.StringSetAsync(_projectName + ":" + key, JsonConvert.SerializeObject(value), expirationTime);
        }

        public async Task<bool> SetValueUnderFolderAsync<T>(string key, string folderName, T value, TimeSpan? expirationTime)
        {
            return await _database.StringSetAsync(_projectName + ":" + folderName + ":" + key, JsonConvert.SerializeObject(value), expirationTime);
        }

        public async Task DeleteByPatternAsync(string pattern)
        {
            foreach (var server in _servers)
            {
                var keys = server.Keys(0, _projectName + ":" + pattern).ToArray();
                await _database.KeyDeleteAsync(keys);
            }
        }


        public T? GetValue<T>(string key)
        {
            var data = _database.StringGet(key);
            if (data.HasValue)
                return JsonConvert.DeserializeObject<T>(data.ToString());
            return default;
        }

        public bool DeleteValue(string key)
        {
            var valueExists = _database.KeyExists(key);
            if (valueExists)
                return _database.KeyDelete(key);
            return false;
        }

        public bool SetValue<T>(string key, T value, TimeSpan? expirationTime)
        {
            return _database.StringSet(_projectName + ":" + key, JsonConvert.SerializeObject(value), expirationTime);
        }

        public bool SetValueUnderFolder<T>(string key, string folderName, T value, TimeSpan? expirationTime)
        {
            return _database.StringSet(_projectName + ":" + folderName + ":" + key, JsonConvert.SerializeObject(value), expirationTime);
        }

        public void DeleteByPattern(string pattern)
        {
            foreach (var server in _servers)
            {
                var keys = server.Keys(0, _projectName + ":" + pattern).ToArray();
                _database.KeyDelete(keys);
            }
        }
    }
}
