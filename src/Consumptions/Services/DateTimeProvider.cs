using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;
    
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}