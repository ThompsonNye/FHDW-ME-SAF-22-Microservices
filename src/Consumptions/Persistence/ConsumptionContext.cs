using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Persistence;

/// <summary>
/// The default DbContext for the Consumptions microservice.
/// </summary>
public class ConsumptionContext : DbContext, IApplicationDbContext
{
    /// <inheritdoc />
    public DbSet<Consumption> Consumptions => Set<Consumption>();

    /// <summary>
    /// The default DbContext configuration for the Consumptions microservice.
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
        modelBuilder.Entity<Consumption>()
            .Property(p => p.Id)
            .HasConversion(x => x.Value, x => new(x));
        
        modelBuilder.Entity<Consumption>()
            .Property(p => p.CarId)
            .HasConversion(x => x.Value, x => new(x));
    }

    /// <summary>
    /// The default save logic for the Consumptions microservice.
    /// For all changed entities, the time is converted to UTC.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    async Task<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var changedConsumption in ChangeTracker.Entries<Consumption>())
        {
            changedConsumption.Entity.DateTime = changedConsumption.Entity.DateTime.ToUniversalTime();
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}