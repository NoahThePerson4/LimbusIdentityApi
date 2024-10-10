namespace LimbusIdentityApp.Models
{
    public class IdentityCreate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sinner { get; set; }
        public int Health { get; set; }
        public string? Ineffective { get; set; }
        public string? Fatal { get; set; }
        public int DefenseLevel { get; set; }
        public int MinSpeed { get; set; }
        public int MaxSpeed { get; set; }
        public string? Image { get; set; }
        public List<int> PassiveIds { get; set; }
        public List<int> SkillIds { get; set; }
    }
}
