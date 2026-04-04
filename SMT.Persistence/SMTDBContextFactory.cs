using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SMT.Persistence.SMTConfiguration;

namespace SMT.Persistence;

public class SMTDBContextFactory : IDesignTimeDbContextFactory<SMTDBContext>
{
    public SMTDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SMTDBContext>();
        
        // Используем строку подключения из appsettings.Development.json
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=SMTDb;Username=postgres;Password=admin");
        
        return new SMTDBContext(optionsBuilder.Options);
    }
}
