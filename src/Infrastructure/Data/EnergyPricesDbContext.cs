using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class EnergyPricesDbContext : DbContext
{
    public EnergyPricesDbContext(DbContextOptions<EnergyPricesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Area> Areas { get; set; }
}
