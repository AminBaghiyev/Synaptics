using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Synaptics.Application.Interfaces;
using Synaptics.Application.Interfaces.Repositories;
using Synaptics.Persistence.Data;
using Synaptics.Persistence.Repositories;
using Synaptics.Persistence.Services;

namespace Synaptics.Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("local")));

        services.AddScoped<IPostRepository, PostRepository>();

        services.AddScoped<IAppUserService, AppUserService>();
        services.AddScoped<IPostService, PostService>();
    }
}
