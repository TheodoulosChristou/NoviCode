using Microsoft.EntityFrameworkCore;
using NoviCode.Configurations;
using NoviCode.Entities;

namespace NoviCode.NoviCodeDbContext
{
    public class NoviCodeDbContext : DbContext
    {

      public NoviCodeDbContext(DbContextOptions options) : base(options)
      {

      }
        
       public DbSet<CurrencyRate> CurrencyRate { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CurrencyRateConfiguration());
        }

    }
}
