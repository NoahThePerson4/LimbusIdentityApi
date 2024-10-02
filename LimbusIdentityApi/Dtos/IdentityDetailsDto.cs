namespace LimbusIdentityApi.Dtos
{
    public record IdentityDetailsDto(
        int Id,
        string Name,
        string Sinner,
        int Health,
        int DefenseLevel,
        int MinSpeed,
        int MaxSpeed,
        string? Image,
        List<int> PassiveIds,
        List<string> PassiveNames,
        List<string?> PassiveCosts,
        List<string> PassiveDescription,
        List<int> SkillIds,
        List<string> SkillNames,
        List<string>? SkillTypes,
        List<string>? SkillSins,
        List<int> SkillMinRolls,
        List<int> SkillMaxRolls,
        List<string?> SkillEffects,
        List<string> CoinEffects
    );
}
