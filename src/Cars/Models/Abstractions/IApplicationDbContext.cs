using Cars.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Nuyken.Vegasco.Backend.Microservices.Cars.Models.Abstractions;

public interface IApplicationDbContext
{
    /// <summary>
    /// The <see cref="Car"/> entries.
    /// </summary>
    DbSet<Car> Cars { get; }
    
    /// <summary>
    /// Asynchronously saves all changes made in this context to the database. 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}