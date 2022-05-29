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
}