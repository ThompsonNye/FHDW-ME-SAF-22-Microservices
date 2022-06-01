using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Configuration;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Requests;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Validation;

/// <summary>
/// Validation for a <see cref="UpdateConsumptionCommand"/>.
/// </summary>
public class UpdateConsumptionCommandValidator : AbstractValidator<UpdateConsumptionCommand>
{
    public UpdateConsumptionCommandValidator(IDateTimeProvider dateTimeProvider,
        IOptions<ValidationOptions> validationOptions,
        IActionContextAccessor actionContextAccessor)
    {
        RuleFor(x => x.Id)
            .NotEqual(default(Guid))
            .When(x => x.Id.HasValue);
        
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .When(x => x.Amount.HasValue);
        
        RuleFor(x => x.Distance)
            .GreaterThan(0)
            .When(x => x.Distance.HasValue);

        RuleFor(x => x.DateTime!.Value.ToUniversalTime())
            .LessThanOrEqualTo(dateTimeProvider.UtcNow.AddMinutes(GetLeeway(validationOptions.Value)))
            .When(x => x.DateTime.HasValue)
            .WithName(nameof(UpdateConsumptionCommand.DateTime));

        RuleFor(x => x.CarId)
            .NotEqual(default(Guid))
            .When(x => x.CarId.HasValue);
        
        RuleFor(x => x.Id)
            .Must(x => MatchRouteId(x!.Value, actionContextAccessor))
            .When(x => x.Id.HasValue)
            .WithMessage("The body's id does not match the route id in the request path");
    }

    private static double GetLeeway(ValidationOptions validationOptions)
    {
        return validationOptions.DateTimeLeewayMinutes >= 0
            ? validationOptions.DateTimeLeewayMinutes
            : 0;
    }

    private static bool MatchRouteId(Guid id, IActionContextAccessor actionContextAccessor)
    {
        if (actionContextAccessor.ActionContext is null)
            return false;

        var couldGetRouteId = actionContextAccessor.ActionContext.RouteData.Values.TryGetValue("id", out var routeId);

        if (!couldGetRouteId)
        {
            return false;
        }
        
        return routeId?.ToString() is { } routeIdString && routeIdString == id.ToString();
    }
}