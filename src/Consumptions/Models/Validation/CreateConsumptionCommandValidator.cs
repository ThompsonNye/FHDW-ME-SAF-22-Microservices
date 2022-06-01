using FluentValidation;
using Microsoft.Extensions.Options;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Configuration;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Requests;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Validation;

/// <summary>
/// Validation for a <see cref="CreateConsumptionCommand"/>.
/// </summary>
public class CreateConsumptionCommandValidator : AbstractValidator<CreateConsumptionCommand>
{
    public CreateConsumptionCommandValidator(IDateTimeProvider dateTimeProvider,
        IOptions<ValidationOptions> validationOptions)
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Distance)
            .GreaterThan(0);

        RuleFor(x => x.DateTime.ToUniversalTime())
            .LessThanOrEqualTo(dateTimeProvider.UtcNow.AddMinutes(GetLeeway(validationOptions.Value)))
            .WithName(nameof(CreateConsumptionCommand.DateTime));

        RuleFor(x => x.CarId)
            .NotEqual(new CarId(default));
    }
    
    private static double GetLeeway(ValidationOptions validationOptions)
    {
        return validationOptions.DateTimeLeewayMinutes >= 0 
            ? validationOptions.DateTimeLeewayMinutes 
            : 0;
    }
}