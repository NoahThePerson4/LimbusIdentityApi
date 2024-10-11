using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models.Custom_Validation
{
    public class ResistAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var idDetails = (IdentityCreate)validationContext.ObjectInstance;

            if (idDetails.Ineffective == "Slash" || idDetails.Ineffective == "Pierce" || idDetails.Ineffective == "Blunt")
            {
                return ValidationResult.Success;

            }
            return new ValidationResult("You must have a resistance of Slash, Pierce, or Blunt.");
        }
    }
}
