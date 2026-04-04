using SMT.Persistence.SMTConfiguration;
using Microsoft.EntityFrameworkCore;

namespace SMT.Persistence;

public class DbInitialize
{
    public static void Initialize(SMTDBContext context)
    {
        // Для разработки: автоматически создаёт таблицы если их нет
        // Для production рекомендуется использовать миграции: context.Database.Migrate();
        context.Database.EnsureCreated();
    }
}