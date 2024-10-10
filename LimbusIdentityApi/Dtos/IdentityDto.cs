namespace LimbusIdentityApi.Dtos
{
    public record IdentityDto(
        int Id,
        string Name,
        string Sinner,
        int Health,
        string? Ineffective,
        string? Fatal,
        int DefenseLevel,
        int MinSpeed,
        int MaxSpeed,
        string? Image,
        List<string>? PassivesName,
        List<string>? SkillNames
        );
    
    
}
