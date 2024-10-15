using FluentValidation;
using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;
using LimbusIdentityApi.Extensions;
using LimbusIdentityApi.Repositories;
using LimbusIdentityApi.Validations;
using Microsoft.AspNetCore.Http.HttpResults;
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

            group.MapGet("", async (ISkillRepository repository, [AsParameters] GetSkillDto request, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SkillEndpoints));
                var skills = (await repository.GetAllSkills(request.filter, request.pageNumber, request.pageSize)).Select(skill => skill.AsSkillDto());

                logger.LogInformation("Get On Skills was called for {pages} skills on page {page} with the filter {filter}.", request.pageSize, request.pageNumber, request.filter);
                return Results.Ok(skills);
            });

            group.MapGet("{id}", async (int id, ISkillRepository repository, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SkillEndpoints));
                var skill = await repository.GetSkill(id);

                if (skill is null)
                {
                    logger.LogError("Get was called on Skills for the Skill with Id {id} but no Skill with that Id exists!", id);
                    return Results.NotFound("No Skill with that Id exists!");
                }

                return Results.Ok(skill.AsSkillDto());
            })
                .WithName(GetSkill);

            group.MapPost("", async (ISkillRepository repository, CreateSkillDto skillDto, ILoggerFactory loggerFactory, IValidator<CreateSkillDto> createSkillValidator) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SkillEndpoints));
                Skill skill = new()
                {
                    Image = skillDto.Image,
                    Name = skillDto.Name,
                    Type = skillDto.Type,
                    Sin = skillDto.Sin,
                    OffenseLevel = skillDto.OffenseLevel,
                    MinRoll = skillDto.MinRoll,
                    MaxRoll = skillDto.MaxRoll,
                    SkillEffect = skillDto.SkillEffect,
                    CoinEffects = skillDto.CoinEffects
                };

                var validationResult = await createSkillValidator.ValidateAsync(skillDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                await repository.CreateSkill(skill);

                logger.LogInformation("The new Skill {name} was created with the Id {id}.", skill.Name, skill.Id);
                return Results.CreatedAtRoute(GetSkill, new { id = skill.Id }, skill.AsSkillDto());
            });

            group.MapPut("{id}", async (int id, ISkillRepository repository, CreateSkillDto updateSkillDto, ILoggerFactory loggerFactory, IValidator<CreateSkillDto> updateSkillValidator) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SkillEndpoints));
                var skill = await repository.GetSkill(id);

                if (skill is null)
                {
                    logger.LogError("Put was called for the Skill with Id {id} but no Skill with that Id exists!", id);
                    return Results.NotFound("No Skill with that Id exists!");
                }

                var validationResult = await updateSkillValidator.ValidateAsync(updateSkillDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                skill.Image = updateSkillDto.Image;
                skill.Name = updateSkillDto.Name;
                skill.Type = updateSkillDto.Type;
                skill.Sin = updateSkillDto.Sin;
                skill.OffenseLevel = updateSkillDto.OffenseLevel;
                skill.MinRoll = updateSkillDto.MinRoll;
                skill.MaxRoll = updateSkillDto.MaxRoll;
                skill.SkillEffect = updateSkillDto.SkillEffect;
                skill.CoinEffects = updateSkillDto.CoinEffects;

                await repository.UpdateSkill(skill);

                logger.LogInformation("The Skill {name} was updated with the Id {Id}.", skill.Name, id);
                return Results.NoContent();
            });

            group.MapDelete("{id}", async (int id, ISkillRepository repository, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(SkillEndpoints));
                var skill = await repository.GetSkill(id);

                if (skill is not null)
                {
                    await repository.DeleteSkill(id);
                }
                logger.LogInformation("The Skill with Id {id} was deleted.", id);
                return Results.NoContent();
            });

            return group;
        }
    }
}
