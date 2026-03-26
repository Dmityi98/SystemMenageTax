using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SMT.Persistence.SMTConfiguration;
using Npgsql;
using SMT.Application.Interfaces;

namespace SMT.Persistence;

public static class DI
{
    public static IServiceCollection AddPersistence(this IServiceCollection service,
        IConfiguration configuration)
    {
        var connection = configuration["DbConnection"];
        service.AddDbContext<SMTDBContext>(options =>
        {
            options.UseNpgsql(connection);
        });
        service.AddScoped<ISMTDBContext>(provider => provider.GetService<SMTDBContext>());
        return service;
    }
}