using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Requests;

/// <summary>
/// Request for creating a <see cref="Consumption"/> entity with the given values.
/// </summary>
public class CreateConsumptionCommand
{
    public DateTimeOffset DateTime { get; init; }

    public int Distance { get; init; }

    public int Amount { get; init; }

    public bool IgnoreInCalculation { get; init; }

    public CarId CarId { get; init; }
}