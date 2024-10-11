using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models.Custom_Validation
{
    public class MinMaxCoinRollAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var skillDetails = (SkillDetails)validationContext.ObjectInstance;

            if (skillDetails.MinRoll > skillDetails.MaxRoll)
            {
                return new ValidationResult("You can't have a higher Min Roll than Max Roll.");
            }

            return ValidationResult.Success;
        }
    }
}
