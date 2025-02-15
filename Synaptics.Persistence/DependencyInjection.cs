using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Synaptics.Application.Interfaces;
using Synaptics.Persistence.Data;
using Synaptics.Persistence.Services;

namespace Synaptics.Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("local")));

        services.AddScoped<IAppUserService, AppUserService>();
    }
}
