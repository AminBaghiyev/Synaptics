using Microsoft.Extensions.DependencyInjection;
using Synaptics.Application.Interfaces;
using Synaptics.Infrastructure.Services;

namespace Synaptics.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IJWTTokenService, JWTTokenService>();
        services.AddSingleton<IFileService, FileService>();
    }
}
