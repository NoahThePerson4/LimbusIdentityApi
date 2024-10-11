using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models.Custom_Validation
{
    public class CoinTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var skillDetails = (SkillDetails)validationContext.ObjectInstance;

            if (skillDetails.Type == "Slash" || skillDetails.Type == "Pierce" || skillDetails.Type == "Blunt")
            {
                return ValidationResult.Success;

            }
            return new ValidationResult("You must have a Type of Slash, Pierce, or Blunt.");
        }
    }
}
