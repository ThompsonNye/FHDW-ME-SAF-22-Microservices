namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

public class Consumption
{
    public ConsumptionId Id { get; set; } = ConsumptionId.New();

    public DateTimeOffset DateTime { get; set; }

    public int Distance { get; set; }

    public int Amount { get; set; }

    public bool IgnoreInCalculation { get; set; }

    public CarId CarId { get; set; }
}