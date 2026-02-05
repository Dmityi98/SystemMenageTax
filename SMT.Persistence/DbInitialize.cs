using SMT.Persistence.SMTConfiguration;
using Microsoft.EntityFrameworkCore;

namespace SMT.Persistence;
 
public class DbInitialize
{
    public static void Initialize(SMTDBContext context)
    {
        // Для production рекомендуется использовать миграции EF Core вместо EnsureCreated
        // Выполните: dotnet ef migrations add InitialCreate && dotnet ef database update
        context.Database.Migrate();   
    }
}