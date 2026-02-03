
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SMT.Application;

public static class DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(assembly);
        });
        
        services.AddAutoMapper(assembly);
        return services;
    }
}