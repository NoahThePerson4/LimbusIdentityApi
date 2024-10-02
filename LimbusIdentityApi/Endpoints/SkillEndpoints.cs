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

            group.MapGet("", async(IdentityDbContext dbContext, [AsParameters] GetSkillDto skillDto, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SkillEndpoints));
                var skill = new List<Skill>();
                if (skillDto.filter is not null)
                {
                    skill = await dbContext.Skills.Where(skill => skill.Name.Contains(skillDto.filter))
                   .OrderBy(skill => skill.Id)
                   .Skip((skillDto.pageNumber - 1) * skillDto.pageSize)
                   .Take(skillDto.pageSize)
                   .ToListAsync();
                }
                else
                {
                    skill = await dbContext.Skills
                    .OrderBy(skill => skill.Id)
                    .Skip((skillDto.pageNumber - 1) * skillDto.pageSize)
                    .Take(skillDto.pageSize)
                    .ToListAsync();
                }
                var skillDtos = skill.Select(skill => skill.AsSkillDto());
                logger.LogInformation("Get On Skills was called for {pages} skills on page {page} with the filter {filter}.", skillDto.pageSize, skillDto.pageNumber, skillDto.filter);
                return Results.Ok(skillDtos);
            });

            group.MapGet("{id}", async (int id ,IdentityDbContext dbContext, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SkillEndpoints));
                var skill = await dbContext.Skills.FindAsync(id);

                if(skill is null)
                {
                    logger.LogError("Get was called on Skills for the Skill with Id {id} but no Skill with that Id exists!", id);
                    return Results.NotFound("No Skill with that Id exists!");
                }

                return Results.Ok(skill.AsSkillDto());
            })
                .WithName(GetSkill);

            group.MapPost("", async (IdentityDbContext dbContext, CreateSkillDto skillDto, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SkillEndpoints));
                Skill skill = new()
                {
                    Name = skillDto.Name,
                    Type = skillDto.Type,
                    Sin = skillDto.Sin,
                    MinRoll = skillDto.MinRoll,
                    MaxRoll = skillDto.MaxRoll,
                    SkillEffect = skillDto.SkillEffect,
                    CoinEffects = skillDto.CoinEffects
                };

                await dbContext.AddAsync(skill);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("The new Skill {name} was created with the Id {id}.",skill.Name, skill.Id);
                return Results.CreatedAtRoute(GetSkill, new { id = skill.Id }, skill.AsSkillDto());
            });

            group.MapPut("{id}", async (int id, IdentityDbContext dbContext, CreateSkillDto updateSkillDto, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SkillEndpoints));
                var skill = await dbContext.Skills.FindAsync(id);

                if(skill is null)
                {
                    logger.LogError("Put was called for the Skill with Id {id} but no Skill with that Id exists!", id);
                    return Results.NotFound("No Skill with that Id exists!");
                }

                skill.Name = updateSkillDto.Name;
                skill.Type = updateSkillDto.Type;
                skill.Sin = updateSkillDto.Sin;
                skill.MinRoll = updateSkillDto.MinRoll;
                skill.MaxRoll = updateSkillDto.MaxRoll;
                skill.SkillEffect = updateSkillDto.SkillEffect;
                skill.CoinEffects = updateSkillDto.CoinEffects;

                dbContext.Update(skill);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("The Skill {name} was updated with the Id {Id}.", skill.Name, id);
                return Results.NoContent();
            });

            group.MapDelete("{id}", async (int id, IdentityDbContext dbContext, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SkillEndpoints));
                var skill = await dbContext.Skills.FindAsync(id);

                if(skill is not null)
                {
                    await dbContext.Skills.Where(skill => skill.Id == id)
                    .ExecuteDeleteAsync();
                }
                logger.LogInformation("The Skill with Id {id} was deleted.", id);
                return Results.NoContent();
            });

            return group;
        }
    }
}
