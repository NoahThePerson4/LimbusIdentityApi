using LimbusIdentityApp.Models.Custom_Validation;
using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models
{
    public class IdentityCreate
    {
        [Required]
        public int Id { get; set; }
        [Required (ErrorMessage ="Please provide a Name.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please provide a Sinner.")]
        [Sinner]
        public string Sinner { get; set; }
        [Required(ErrorMessage = "Please provide a Health Value.")]
        [Range(0, 1000, ErrorMessage = "Health should be between 0 and 1000.")]
        public int Health { get; set; }
        [Required(ErrorMessage = "Please provide a Resistance.")]
        [Resist]
        [Same]
        public string Ineffective { get; set; }
        [Required(ErrorMessage = "Please provide a Weakness.")]
        [Weak]
        [Same]
        public string Fatal { get; set; }
        [Required(ErrorMessage = "Please provide a Defense Level (50 is the base).")]
        [Range(0, 200, ErrorMessage ="Defense Level should be between 0 and 200.")]
        public int DefenseLevel { get; set; }
        [Required(ErrorMessage = "Please provide a Min Speed.")]
        [MinMaxSpeed]
        [Range(0,100, ErrorMessage = "Min Speed should be between 0 and 100.")]
        public int MinSpeed { get; set; }
        [Required(ErrorMessage = "Please provide a Max Speed.")]
        [MinMaxSpeed]
        [Range(0, 100, ErrorMessage = "Max Speed should be between 0 and 100.")]
        public int MaxSpeed { get; set; }
        public string? Image { get; set; }
        public List<int> PassiveIds { get; set; }
        public List<int> SkillIds { get; set; }
    }
}
