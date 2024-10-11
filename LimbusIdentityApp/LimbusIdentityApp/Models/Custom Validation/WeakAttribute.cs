using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models.Custom_Validation
{
    public class WeakAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var idDetails = (IdentityCreate)validationContext.ObjectInstance;

            if (idDetails.Fatal == "Slash" || idDetails.Fatal == "Pierce" || idDetails.Fatal == "Blunt")
            {
                return ValidationResult.Success;

            }
            return new ValidationResult("You must have a resistance of Slash, Pierce, or Blunt.");
        }
    }
}
