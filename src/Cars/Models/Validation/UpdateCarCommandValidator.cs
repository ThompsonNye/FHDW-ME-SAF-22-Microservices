using FluentValidation;
using Nuyken.Vegasco.Backend.Microservices.Cars.Models.Requests;

namespace Nuyken.Vegasco.Backend.Microservices.Cars.Models.Validation;

public class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
{
    public UpdateCarCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => x.Name is not null);
    }
}