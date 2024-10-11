using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models.Custom_Validation
{
    public class MinRollMaxRollAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var skillDetails = (SkillDetails)validationContext.ObjectInstance;

            if (skillDetails.MinRoll >= skillDetails.MaxRoll)
            {
                return new ValidationResult("Min Roll must be less than Max Roll.");
            }

            return ValidationResult.Success;
        }
    }
}
