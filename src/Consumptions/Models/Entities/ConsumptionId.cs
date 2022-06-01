namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

/// <summary>
/// A strongly typed, <see cref="Guid"/>-based ID.
/// </summary>
[StronglyTypedId(jsonConverter: StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct ConsumptionId
{
    
}