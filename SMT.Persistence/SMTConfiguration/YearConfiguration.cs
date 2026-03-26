using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMT.Domain.Models;

namespace SMT.Persistence.SMTConfiguration
{
    public class YearConfiguration : IEntityTypeConfiguration<Year>
    {
        public void Configure(EntityTypeBuilder<Year> builder)
        {
            builder.HasKey(y => y.Id);

            builder.Property(y => y.NameTable)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(y => y.TotalForQuartet)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(y => y.User)
                .WithMany(u => u.Years)
                .HasForeignKey(y => y.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(y => y.Quarters)
                .WithOne(q => q.Year)
                .HasForeignKey(q => q.YearID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}