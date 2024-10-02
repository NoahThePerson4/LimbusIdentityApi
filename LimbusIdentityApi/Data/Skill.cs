namespace LimbusIdentityApi.Data
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; }
        public string? Sin { get; set; }
        public int MinRoll { get; set; }
        public int MaxRoll { get; set; }
        public string? SkillEffect { get; set; }
        public List<string>? CoinEffects { get; set; }
    }
}
