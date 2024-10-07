namespace LimbusIdentityApi.Dtos
{
    public record CreateSkillDto(
        string? Image,
        string Name,
        string? Type,
        string? Sin,
        int MinRoll,
        int MaxRoll,
        string? SkillEffect,
        List<string?>? CoinEffects);
}
