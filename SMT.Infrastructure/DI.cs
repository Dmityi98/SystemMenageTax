using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SMT.Persistence.SMTConfiguration;
using Npgsql;
using SMT.Application.Interfaces;
using SMT.Infrastructure.Services;

namespace SMT.Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service,
        IConfiguration configuration)
    {
        service.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        service.AddScoped<IPasswordHasher, PasswordHasher>();
        service.AddScoped<IJwtProvider, JwtProvider>();
        return service;
    }
}