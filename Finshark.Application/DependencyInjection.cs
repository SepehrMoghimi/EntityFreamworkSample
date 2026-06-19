using System.Reflection;
using Finshark.Domain.Interface.Base;
using Finshark.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finshark.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IScopedService).IsAssignableFrom(t)))
        {
            services.AddScoped(type);
        }

        return services;
    }
}
