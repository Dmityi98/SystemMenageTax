using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMT.Domain.Models;

namespace SMT.Persistence.SMTConfiguration;
 
public class SMTConfiguration : 
    IEntityTypeConfiguration<User>,
    IEntityTypeConfiguration<Year>,
    IEntityTypeConfiguration<MonthColumn>,
    IEntityTypeConfiguration<Quarter>
{
    
}