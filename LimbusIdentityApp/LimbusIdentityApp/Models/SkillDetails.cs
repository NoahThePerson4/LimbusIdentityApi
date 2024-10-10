namespace LimbusIdentityApp.Models
{
    public class SkillDetails
    {
        public int Id { get; set; }
        public string? Image { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; }
        public string? Sin { get; set; }
        public int? OffenseLevel { get; set; }
        public int MinRoll { get; set; }
        public int MaxRoll { get; set; }
        public string? SkillEffect { get; set; }
        public List<string>? CoinEffects { get; set; }
    }
}
