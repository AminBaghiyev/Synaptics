namespace Synaptics.Application.Interfaces.Services;

public interface IRedisService
{
    Task SetHashAsync(string key, string field, string metadata, TimeSpan expiry);
    Task<string?> GetFieldFromHashAsync(string key, string field);
    Task<Dictionary<string, string>> GetAllFromHashByKeyAsync(string key, int page, int count = 15);
    Task DeleteHashAsync(string key, string field);
    Task DeleteAllFromHashByKeyAsync(string key);
}
