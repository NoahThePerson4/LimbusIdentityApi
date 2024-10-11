using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models.Custom_Validation
{
    public class MinMaxSpeedAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var identityCreate = (IdentityCreate)validationContext.ObjectInstance;

            if (identityCreate.MinSpeed > identityCreate.MaxSpeed)
            {
                return new ValidationResult("You can't have a higher Min Speed than Max Speed.");
            }

            return ValidationResult.Success;
        }
    }
}
