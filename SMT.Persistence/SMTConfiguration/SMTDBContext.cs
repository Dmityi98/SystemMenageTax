using Microsoft.EntityFrameworkCore;
using SMT.Application.Interfaces;
using SMT.Domain.Models;

namespace SMT.Persistence.SMTConfiguration;

public class SMTDBContext : DbContext, ISMTDBContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Year> Years { get; set; }
    public DbSet<MonthColumn> Months { get; set; }
    public DbSet<Quarter> Quarters { get; set; }

    public SMTDBContext(DbContextOptions<SMTDBContext> options)
        : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SMTConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}