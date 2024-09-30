namespace LimbusIdentityApi.Dtos
{
    public record SkillDto(
        int Id,
        string Name,
        int MinRoll,
        int MaxRoll,
        string? SkillEffect,
        List<string?>? CoinEffects
        );
    
    
}
