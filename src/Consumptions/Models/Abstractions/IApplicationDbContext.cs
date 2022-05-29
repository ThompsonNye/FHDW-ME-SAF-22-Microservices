using Microsoft.EntityFrameworkCore;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Consumption> Consumptions { get; }
}