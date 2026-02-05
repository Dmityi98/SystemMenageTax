using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMT.Domain.Models;

namespace SMT.Persistence.SMTConfiguration
{
    public class QuarterConfiguration : IEntityTypeConfiguration<Quarter>
    {
        public void Configure(EntityTypeBuilder<Quarter> builder)
        {
            builder.HasKey(q => q.Id);

            builder.HasOne(q => q.Year)
                .WithMany(y => y.Quarters)
                .HasForeignKey(q => q.YearID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.Columns)
                .WithOne(c => c.Quarter)
                .HasForeignKey(c => c.QuarterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}