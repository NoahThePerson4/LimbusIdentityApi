namespace LimbusIdentityApp.Models
{
    public class IdentityDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sinner { get; set; }
        public int Health { get; set; }
        public int DefenseLevel { get; set; }
        public int MinSpeed { get; set; }
        public int MaxSpeed { get; set; }
        public string? Image { get; set; }
        public List<int> PassiveIds { get; set; }
        public List<string?> PassiveCosts { get; set; }
        public List<string> PassiveDescription { get; set; }
        public List<int> SkillIds { get; set; }
        public List<string>? PassivesName { get; set; }
        public List<string>? SkillNames { get; set; }
        public List<string>? SkillTypes { get; set; }
        public List<string>? SkillSins { get; set; }
        public List<int> SkillMinRolls { get; set; }
        public List<int> SkillMaxRolls { get; set; }
        public List<string?> SkillEffects { get; set; }
        public List<string> CoinEffects { get; set; }
    }
}
