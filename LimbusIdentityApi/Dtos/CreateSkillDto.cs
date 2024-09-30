namespace LimbusIdentityApi.Dtos
{
    public record CreateSkillDto(
        string Name,
        int MinRoll,
        int MaxRoll,
        string? SkillEffect,
        List<string?>? CoinEffects);
}
