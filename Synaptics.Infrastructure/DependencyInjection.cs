using Microsoft.Extensions.DependencyInjection;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Infrastructure.Services;

namespace Synaptics.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IJWTTokenService, JWTTokenService>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<IPyBridgeService, PyBridgeService>();
        services.AddSingleton<IQdrantService, QdrantService>();
        services.AddSingleton<IRedisService, RedisService>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<IUserDeviceInfoService, UserDeviceInfoService>();
    }
}
