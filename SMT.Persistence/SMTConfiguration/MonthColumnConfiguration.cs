using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMT.Domain.Models;

namespace SMT.Persistence.SMTConfiguration
{
    public class MonthColumnConfiguration : IEntityTypeConfiguration<MonthColumn>
    {
        public void Configure(EntityTypeBuilder<MonthColumn> builder)
        {
            builder.HasKey(mc => mc.Id);

            builder.Property(mc => mc.Turnover)
                .HasColumnType("decimal(18,2)");

            builder.Property(mc => mc.TaxPayable)
                .HasColumnType("decimal(18,2)");

            builder.Property(mc => mc.PaidTax)
                .HasColumnType("decimal(18,2)");

            builder.Property(mc => mc.Month)
                .IsRequired();

            builder.HasOne(mc => mc.Quarter)
                .WithMany(q => q.Columns)
                .HasForeignKey(mc => mc.QuarterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}