using System.Reflection;
using Cars.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Nuyken.Vegasco.Backend.Microservices.Cars.Models.Abstractions;

namespace Nuyken.Vegasco.Backend.Microservices.Cars.Persistence;

public class CarsContext : DbContext, IApplicationDbContext
{
    public DbSet<Car> Cars => Set<Car>();
    
    /// <summary>
    /// The default DbContext configuration for the Cars microservice.
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var name = Assembly.GetExecutingAssembly().FullName ?? "InMemDefaultDb";
        optionsBuilder.UseInMemoryDatabase(name);
    }
    
    /// <summary>
    /// The default DbContext model builder configuration for the Consumptions microservice.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .Property(p => p.Id)
            .HasConversion(x => x.Value, x => new(x));
    }
}