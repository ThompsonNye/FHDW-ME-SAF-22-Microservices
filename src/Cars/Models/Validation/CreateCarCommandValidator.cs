using FluentValidation;
using Nuyken.Vegasco.Backend.Microservices.Cars.Models.Requests;

namespace Nuyken.Vegasco.Backend.Microservices.Cars.Models.Validation;

public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
{
    public CreateCarCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}