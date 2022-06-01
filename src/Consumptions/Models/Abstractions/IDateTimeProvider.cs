namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
    
    DateTimeOffset UtcNow { get; }
}