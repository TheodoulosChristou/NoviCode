using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoviCode.Entities;

namespace NoviCode.Configurations
{
    public class CurrencyRateConfiguration : IEntityTypeConfiguration<CurrencyRate>
    {
        public void Configure(EntityTypeBuilder<CurrencyRate> builder)
        {
            builder.ToTable("CurrencyRate");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.CurrencyCode, x.RateDate }).IsUnique();

            builder.Property(x => x.Rate).HasPrecision(18, 6);
           
        }
    }
}
