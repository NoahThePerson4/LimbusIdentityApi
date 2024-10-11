using FluentValidation;
using LimbusIdentityApi.Dtos;

namespace LimbusIdentityApi.Validations
{
    public class CreatePassiveDtoValidator : AbstractValidator<CreatePassiveDto>
    {
        public CreatePassiveDtoValidator()
        {
            RuleFor(passive => passive.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the passive a Name.")
                .Must(name => name != "string")
                .WithMessage("You left your name on the default Name.");

            RuleFor(passive => passive.Cost)
               .NotNull()
               .NotEmpty()
               .WithMessage("If the passive does not have a cost put Free.")
               .Must(passive => passive != "string")
               .WithMessage("You left your Cost on the default Cost.");

            RuleFor(passive => passive.Support)
                .NotNull()
                .WithMessage("Leave this false if the passive is not a support passive");

            RuleFor(passive => passive.Description)
               .NotNull()
               .NotEmpty()
               .WithMessage("Please give the passive a Description.")
               .Must(passive => passive != "string")
               .WithMessage("You left your Description on the default Description.");
        }
    }
}