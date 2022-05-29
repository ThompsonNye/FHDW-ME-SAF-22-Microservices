using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Persistence;

public class ConsumptionContext : DbContext, IApplicationDbContext
{
    public DbSet<Consumption> Consumptions => Set<Consumption>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var name = Assembly.GetExecutingAssembly().FullName ?? "InMemDefaultDb";
        optionsBuilder.UseInMemoryDatabase(name);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Consumption>()
            .Property(p => p.Id)
            .HasConversion(x => x.Value, x => new(x));
        
        modelBuilder.Entity<Consumption>()
            .Property(p => p.CarId)
            .HasConversion(x => x.Value, x => new(x));
    }

    async Task<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var changedConsumption in ChangeTracker.Entries<Consumption>())
        {
            changedConsumption.Entity.DateTime = changedConsumption.Entity.DateTime.ToUniversalTime();
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}