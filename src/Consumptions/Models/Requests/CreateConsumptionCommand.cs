using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Requests;

public class CreateConsumptionCommand
{
    public DateTimeOffset DateTime { get; init; }

    public int Distance { get; init; }

    public int Amount { get; init; }

    public bool IgnoreInCalculation { get; init; }

    public CarId CarId { get; init; }
}