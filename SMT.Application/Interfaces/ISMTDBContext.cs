using SMT.Domain.Models;
using Microsoft.EntityFrameworkCore; 

namespace SMT.Application.Interfaces;

public interface ISMTDBContext 
{
    DbSet<Domain.Models.User> Users { get; set; }
    DbSet<Year> Years { get; set; }
    DbSet<MonthColumn> Months { get; set; }
    DbSet<Quarter> Quarters { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}