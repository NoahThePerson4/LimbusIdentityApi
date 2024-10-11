using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LimbusIdentityApp.Models.Custom_Validation
{
    public class SinnerAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var idDetails = (IdentityCreate)validationContext.ObjectInstance;
            if (idDetails.Sinner == "Yi Sang" || idDetails.Sinner == "Faust" || idDetails.Sinner == "Don Quixote" || idDetails.Sinner == "Ryoshu"
            || idDetails.Sinner == "Meursault" || idDetails.Sinner == "Hong Lu" || idDetails.Sinner == "Heathcliff" || idDetails.Sinner == "Ishmael" ||
            idDetails.Sinner == "Rodion" || idDetails.Sinner == "Sinclair" || idDetails.Sinner == "Outis" || idDetails.Sinner == "Gregor")
            {
                return ValidationResult.Success;

            }
            return new ValidationResult("Sorry that is not a valid Sinner name. For Rodya please put Rodion. For Ryōshū please put Ryoshu. The 12 Sinners are Yi Sang, Faust, Don Quixote, Ryoshu, Meursault, Hong Lu, Heathcliff, Ishmael, Rodion, Sinclair, Outis, Gregor.");
        }
    }
}
