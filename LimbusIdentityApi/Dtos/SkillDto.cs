namespace LimbusIdentityApi.Dtos
{
    public record SkillDto(
        int Id,
        string Name,
        string? Type,
        string? Sin,
        int MinRoll,
        int MaxRoll,
        string? SkillEffect,
        List<string?>? CoinEffects
        );
}
