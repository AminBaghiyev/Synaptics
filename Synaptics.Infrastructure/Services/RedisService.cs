using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Synaptics.Application.Interfaces.Services;

namespace Synaptics.Infrastructure.Services;

public class RedisService : IRedisService
{
    readonly ConnectionMultiplexer _redisConnection;
    readonly IDatabase _database;

    public RedisService(IConfiguration configuration)
    {
        string endpoint = $"{configuration["Redis:Host"]}:{configuration["Redis:Port"]}";

        ConfigurationOptions options = new()
        {
            EndPoints = { endpoint },
            User = "default",
            Password = configuration["Redis:SecretKey"]
        };

        _redisConnection = ConnectionMultiplexer.Connect(options);
        _database = _redisConnection.GetDatabase();
    }

    public async Task SetHashAsync(string key, string field, string metadata, TimeSpan expiry)
    {
        await _database.HashSetAsync(key, field, metadata);
        await _database.KeyExpireAsync(key, expiry);
    }

    public async Task<string?> GetFieldFromHashAsync(string key, string field) => await _database.HashGetAsync(key, field);

    public async Task<Dictionary<string, string>> GetAllFromHashByKeyAsync(string key, int page, int count = 15)
    {
        int start = page * count;
        int end = start + count - 1;

        IAsyncEnumerable<HashEntry> res = _database.HashScanAsync(key, "*", count, 0);
        ICollection<HashEntry> entries = [];
        await foreach (var entry in res)
            entries.Add(entry);

        return entries.Skip(start).Take(count).ToDictionary(entry => entry.Name.ToString(), entry => entry.Value.ToString());
    }

    public async Task DeleteHashAsync(string key, string field)
    {
        await _database.HashDeleteAsync(key, field);
    }

    public async Task DeleteAllFromHashByKeyAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}
