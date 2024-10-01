﻿using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;

namespace LimbusIdentityApi.Extensions
{
    public static class SkillExtensions
    {
        public static SkillDto AsSkillDto(this Skill skill)
        {
            return new SkillDto(
                skill.Id,
                skill.Name,
                skill.MinRoll,
                skill.MaxRoll,
                skill.SkillEffect,
                skill.CoinEffects?.ToList()
                );                
        }
    }
}
