﻿namespace LimbusIdentityApi.Dtos
{
    public record SkillDto(
        int Id,
        string? Image,
        string Name,
        string Type,
        string Sin,
        int OffenseLevel,
        int MinRoll,
        int MaxRoll,
        string? SkillEffect,
        List<string?>? CoinEffects
        );
}
