using LimbusIdentityApp.Models.Custom_Validation;
using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models
{
    
    public class PassiveDetails
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please provide a Name.")]
        public string Name { get; set; }

        [Required (ErrorMessage = "If the passive has no Cost use Free")]
        public string Cost { get; set; }
        [Required(ErrorMessage = "Leave False if it is not a Support Passive.")]
        public bool Support { get; set; }
        [Required(ErrorMessage = "Please provide a Description.")]
        public string Description { get; set; }
    }
}
