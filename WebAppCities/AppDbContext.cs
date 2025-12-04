namespace WebAppCities;

// Data/* layer
using Microsoft.EntityFrameworkCore;
using WebAppCities.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<City> Cities => Set<City>();
}
