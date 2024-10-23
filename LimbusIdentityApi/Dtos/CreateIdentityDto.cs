namespace LimbusIdentityApi.Dtos
{
    public record CreateIdentityDto(
        string Name,
        string Sinner,
        int Health,
        string Ineffective,
        string Fatal,
        int DefenseLevel,
        int MinSpeed,
        int MaxSpeed,
        string? Faction,
        string? Image,
        List<int>? PassiveIds,
        List<int>? SkillIds
        );


}
