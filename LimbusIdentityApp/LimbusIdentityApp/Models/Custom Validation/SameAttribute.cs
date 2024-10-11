using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models.Custom_Validation
{
    public class SameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var idDetails = (IdentityCreate)validationContext.ObjectInstance;

            if (idDetails.Fatal == idDetails.Ineffective)
            {
                return new ValidationResult("You can't both Resist and be Weak to The Same Damage Type.");
            }
            
            return ValidationResult.Success;
        }
    }
}
