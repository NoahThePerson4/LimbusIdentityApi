using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models.Custom_Validation
{
    public class ValidSinAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var skillDetails = (SkillDetails)validationContext.ObjectInstance;

            if (skillDetails.Sin == "Wrath" || skillDetails.Sin == "Lust" || skillDetails.Sin == "Sloth" 
                || skillDetails.Sin == "Gluttony" || skillDetails.Sin == "Pride" || skillDetails.Sin == "Gloom"
                || skillDetails.Sin == "Envy")
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("You must have a Sin Type of Wrath, Lust, Sloth, Gluttony, Pride, Gloom, or Envy.");
        }
    }
}
