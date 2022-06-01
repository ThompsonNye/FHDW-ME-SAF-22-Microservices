namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Requests;

public class UpdateConsumptionCommand
{
    public Guid? Id { get; set; }
    
    public DateTimeOffset? DateTime { get; init; }

    public int? Distance { get; init; }

    public int? Amount { get; init; }

    public bool? IgnoreInCalculation { get; init; }

    public Guid? CarId { get; init; }
}