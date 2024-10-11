using LimbusIdentityApp.Models.Custom_Validation;
using System.ComponentModel.DataAnnotations;

namespace LimbusIdentityApp.Models
{
    [MinRollMaxRoll]
    public class SkillDetails
    {
        [Required]
        public int Id { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Please provide a Name.")]
        public string Name { get; set; }
        [CoinType]
        [Required(ErrorMessage = "Please provide a Coin Type.")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please provide a Sin Type.")]
        [ValidSin]
        public string Sin { get; set; }
        [Range(1, 200, ErrorMessage = "Offense Level Must be between 1 and 200.")]
        [Required(ErrorMessage = "Please provide an Offense Level (Base is 50).")]
        public int OffenseLevel { get; set; }
        [MinMaxCoinRoll]
        [Required(ErrorMessage = "Please provide a Min Roll.")]
        [Range(1, 100, ErrorMessage = "Min Roll Must be between 1 and 100.")]
        public int MinRoll { get; set; }
        [Range(1, 100, ErrorMessage = "Max Roll Must be between 1 and 100.")]
        [MinMaxCoinRoll]
        [Required(ErrorMessage = "Please provide a Max Roll.")]
        public int MaxRoll { get; set; }
        public string? SkillEffect { get; set; }
        public List<string>? CoinEffects { get; set; }
    }
}
