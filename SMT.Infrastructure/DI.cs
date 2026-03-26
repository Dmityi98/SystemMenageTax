using Microsoft.Extensions.DependencyInjection;
using SMT.Application.Interfaces;
using SMT.Infrastructure.Services;

namespace SMT.Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }
}
