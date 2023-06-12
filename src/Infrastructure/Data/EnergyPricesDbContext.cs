using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class EnergyPricesDbContext : DbContext
{
    public EnergyPricesDbContext(DbContextOptions<EnergyPricesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    public DbSet<Area> Areas { get; set; }
    public DbSet<DailyPriceCollection> DailyPrices { get; set; }
    public DbSet<HourlyPrice> HourlyPrices { get; set; }
}
