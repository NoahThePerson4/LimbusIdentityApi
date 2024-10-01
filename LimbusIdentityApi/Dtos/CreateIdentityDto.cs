namespace LimbusIdentityApi.Dtos
{
    public record CreateIdentityDto(
        string Name,
        string Sinner,
        int Health,
        int DefenseLevel,
        int MinSpeed,
        int MaxSpeed,
        string? Image,
        List<int>? PassiveIds,
        List<int>? SkillIds
        );
    
    
}
