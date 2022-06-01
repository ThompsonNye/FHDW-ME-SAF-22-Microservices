namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;

/// <summary>
/// Provides the current date and time including the time zone / offset.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// The current local time.
    /// </summary>
    DateTimeOffset Now { get; }
    
    /// <summary>
    /// The current UTC time.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}