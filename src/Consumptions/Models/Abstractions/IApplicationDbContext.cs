using Microsoft.EntityFrameworkCore;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;

public interface IApplicationDbContext
{
    /// <summary>
    /// The consumption entries.
    /// </summary>
    DbSet<Consumption> Consumptions { get; }
    
    /// <summary>
    /// Persists any tracked changes to the database.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}