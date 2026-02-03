using SMT.Persistence.SMTConfiguration;

namespace SMT.Persistence;

public class DbInitialize
{
    public static void Initialize(SMTDBContext context)
    {
        context.Database.EnsureCreated();   
    }
}