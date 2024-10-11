using FluentValidation;
using LimbusIdentityApi.Dtos;

namespace LimbusIdentityApi.Validations
{
    public class CreateIdentityDtoValidator : AbstractValidator<CreateIdentityDto>
    {
        public CreateIdentityDtoValidator()
        {
            RuleFor(id => id.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the identity a Name.")
                .Must(name => name != "string")
                .WithMessage("You left your name on the default Name.");

            RuleFor(id => id.Sinner)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please say what Sinner this identity is for.")
                .Must(name => name == "Yi Sang" || name == "Faust" || name == "Don Quixote" || name == "Ryoshu"
                || name == "Meursault" || name == "Hong Lu" || name == "Heathcliff" || name == "Ishmael" ||
                name == "Rodion" || name == "Sinclair" || name == "Outis" || name == "Gregor")
                .WithMessage("Sorry that is not a valid Sinner name. For Rodya please put Rodion. For Ryōshū please put Ryoshu. The 12 Sinners are Yi Sang, Faust, Don Quixote, Ryoshu, Meursault, Hong Lu, Heathcliff, Ishmael, Rodion, Sinclair, Outis, Gregor.");

            RuleFor(id => id.Health)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the identity Health.")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Health can't be negative.");

            RuleFor(id => id.Ineffective)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the identity a Resistance.")
                .Must(id => id == "Slash" || id == "Pierce" || id == "Blunt")
                .WithMessage("Resistance must be either Slash, Pierce, or Blunt.")
                .NotEqual(id => id.Fatal)
                .WithMessage("You can't both be Fatal and Ineffective to the same Damage Type.");

            RuleFor(id => id.Fatal)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the identity a Weakness.")
                .Must(id => id == "Slash" || id == "Pierce" || id == "Blunt")
                .WithMessage("Weakness must be either Slash, Pierce, or Blunt.");

            RuleFor(id => id.DefenseLevel)
                .NotNull()
                .NotEmpty()
                .WithMessage("Please give the identity a Defense Level (base is 50).")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Defense Level can't be negative.");

            RuleFor(id => id.MinSpeed)
               .NotNull()
               .NotEmpty()
               .WithMessage("Please give the identity a Minimum Speed Value.")
               .GreaterThanOrEqualTo(0)
               .WithMessage("Minimum Speed can't be negative.")
               .LessThan(id => id.MaxSpeed)
               .WithMessage("Minimum Speed must be lower than Maximum Speed.");

            RuleFor(id => id.MaxSpeed)
               .NotNull()
               .NotEmpty()
               .WithMessage("Please give the identity a Maximum Speed Value.")
               .GreaterThanOrEqualTo(0)
               .WithMessage("Maximum Speed can't be negative.");

            RuleFor(id => id.Image)
               .Must(id => Uri.IsWellFormedUriString(id, UriKind.Absolute))
               .WithMessage("If you provide an Image you must provide a valid image. Leave empty if you don't want an image.")
               .When(id => !string.IsNullOrEmpty(id.Image));
        }
    }
}
