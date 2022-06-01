using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Services;

/// <inheritdoc />
public class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc />
    public DateTimeOffset Now => DateTimeOffset.Now;
    
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}