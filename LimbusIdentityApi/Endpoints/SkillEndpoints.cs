using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;
using LimbusIdentityApi.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace LimbusIdentityApi.Endpoints
{
    public static class SkillEndpoints
    {
        public const string GetSkill = "GetSkill";
        public static RouteGroupBuilder MapSkillEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/Skills")
                .WithTags("Skills");

            group.MapGet("", (IdentityDbContext dbContext) =>
            {
                var skills = dbContext.Skills.ToList();
                var skillDto = skills.Select(skill => skill.AsSkillDto());
                return Results.Ok(skillDto);
            });

            group.MapGet("{id}", async (int id ,IdentityDbContext dbContext) =>
            {
                var skill = await dbContext.Skills.FindAsync(id);

                if(skill is null)
                {
                    return Results.NotFound("");
                }

                return Results.Ok(skill.AsSkillDto());
            })
                .WithName(GetSkill);

            group.MapPost("", async (IdentityDbContext dbContext, CreateSkillDto skillDto) =>
            {
                Skill skill = new()
                {
                    Name = skillDto.Name,
                    MinRoll = skillDto.MinRoll,
                    MaxRoll = skillDto.MaxRoll,
                    SkillEffect = skillDto.SkillEffect,
                    CoinEffects = skillDto.CoinEffects
                };

                await dbContext.AddAsync(skill);
                await dbContext.SaveChangesAsync();

                return Results.CreatedAtRoute(GetSkill, new { id = skill.Id }, skill.AsSkillDto());
            });

            group.MapPut("{id}", async (int id, IdentityDbContext dbContext, CreateSkillDto updateSkillDto) =>
            {
                var skill = await dbContext.Skills.FindAsync(id);

                if(skill is null)
                {
                    return Results.NotFound("");
                }

                skill.Name = updateSkillDto.Name;
                skill.MinRoll = updateSkillDto.MinRoll;
                skill.MaxRoll = updateSkillDto.MaxRoll;
                skill.SkillEffect = updateSkillDto.SkillEffect;
                skill.CoinEffects = updateSkillDto.CoinEffects;

                dbContext.Update(skill);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

            group.MapDelete("{id}", async (int id, IdentityDbContext dbContext) =>
            {
                var skill = await dbContext.Skills.FindAsync(id);

                if(skill is not null)
                {
                    await dbContext.Skills.Where(skill => skill.Id == id)
                    .ExecuteDeleteAsync();
                }
                return Results.NoContent();
            });

            return group;
        }
    }
}
