using FluentValidation;
using LimbusIdentityApi.Dtos;

namespace LimbusIdentityApi.Validations
{
    public class CreateSkillDtoValidator : AbstractValidator<CreateSkillDto>
    {
        public CreateSkillDtoValidator()
        {
            RuleFor(skill => skill.Image)
               .Must(skill => Uri.IsWellFormedUriString(skill, UriKind.Absolute))
               .WithMessage("If you provide an Image you must provide a valid image. Leave empty if you don't want an image.")
               .When(skill => !string.IsNullOrEmpty(skill.Image));

            RuleFor(skill => skill.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the skill a name.")
                .Must(skill => skill != "string")
                .WithMessage("You left the skill on the default name.");

            RuleFor(skill => skill.Type)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the skill a Coin Type.")
                .Must(skill => skill == "Slash" || skill == "Pierce" || skill == "Blunt")
                .WithMessage("Coin Type must be either Slash, Pierce, or Blunt.");

            RuleFor(skill => skill.Sin)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the skill a Sin Type.")
                .Must(skill => skill == "Wrath" || skill == "Lust" || skill == "Sloth" || skill == "Gluttony" || skill == "Gloom" || skill == "Pride" || skill == "Envy")
                .WithMessage("Sin Type must be either Wrath. Lust, Sloth, Gluttony, Gloom, Pride, or Envy.");

            RuleFor(skill => skill.OffenseLevel)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the skill an Offense Level (50 is the base).")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Offense Level can't be negative.");

            RuleFor(skill => skill.MinRoll)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the skill a Min Roll.")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min Roll can't be negative.");

            RuleFor(skill => skill.MinRoll)
               .LessThan(skill => skill.MaxRoll)
               .WithMessage("Min Roll can't be higher than your Max Roll.");

            RuleFor(skill => skill.MaxRoll)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the skill a Max Roll.")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Max Roll can't be negative.");
        }
    }
}
