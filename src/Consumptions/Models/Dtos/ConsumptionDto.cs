namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Dtos;

public class ConsumptionDto
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset DateTime { get; set; }

    public int Distance { get; set; }

    public int Amount { get; set; }

    public bool IgnoreInCalculation { get; set; }

    public Guid CarId { get; set; }

}