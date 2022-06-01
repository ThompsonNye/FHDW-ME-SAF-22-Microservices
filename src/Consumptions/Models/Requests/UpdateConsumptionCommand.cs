using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Requests;

/// <summary>
/// Request for updating a <see cref="Consumption"/> with the values supplied.
/// </summary>
public class UpdateConsumptionCommand
{
    public Guid? Id { get; set; }
    
    public DateTimeOffset? DateTime { get; init; }

    public int? Distance { get; init; }

    public int? Amount { get; init; }

    public bool? IgnoreInCalculation { get; init; }

    public Guid? CarId { get; init; }
}