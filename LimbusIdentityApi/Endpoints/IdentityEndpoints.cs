using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;
using LimbusIdentityApi.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LimbusIdentityApi.Endpoints
{
    public static class IdentityEndpoints
    {
        public const string GetIdentity = "GetIdentity";
        public static RouteGroupBuilder MapIdentityEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/Identities")
                .WithTags("Identities");

            group.MapGet("", (IdentityDbContext dbContext) =>
            {
                var identities = dbContext.Identities.Include(identity => identity.Passives).Include(identity=> identity.Skills).ToList();
                var identitiesDto = identities.Select(identity => identity.AsIdentityDto());
                return Results.Ok(identitiesDto);
            });

            group.MapGet("{id}", async (int id, IdentityDbContext dbContext) =>
            {
                var identity = await dbContext.Identities.FindAsync(id);

                if (identity is null)
                {
                    return Results.NotFound("");
                }
                return Results.Ok(identity.AsIdentityDto());
            })
                .WithName(GetIdentity);

            group.MapPost("", async (IdentityDbContext dbContext, CreateIdentityDto identityDto) =>
            {
                Identity identity = new()
                {
                    Name = identityDto.Name,
                    Sinner = identityDto.Sinner,
                    Health = identityDto.Health,
                    DefenseLevel = identityDto.DefenseLevel,
                    MinSpeed = identityDto.MinSpeed,
                    MaxSpeed = identityDto.MaxSpeed,
                    Image = identityDto.Image
                };

                if(identityDto.PassiveIds is not null)
                {
                    var passives = await dbContext.Passives
                    .Where(p => identityDto.PassiveIds.Contains(p.Id))
                    .ToListAsync();

                    identity.Passives = passives;
                }

                if(identityDto.SkillIds is not null)
                {
                    var skills = await dbContext.Skills
                    .Where(s => identityDto.SkillIds.Contains(s.Id))
                    .ToListAsync();

                    identity.Skills = skills;
                }

                await dbContext.AddAsync(identity);
                await dbContext.SaveChangesAsync();

                return Results.CreatedAtRoute(GetIdentity, new { id = identity.Id }, identity.AsIdentityDto());
            });

            group.MapPut("{id}", async (int id, IdentityDbContext dbContext, CreateIdentityDto updateIdentityDto) =>
            {
                var identity = await dbContext.Identities.FindAsync(id);

                if(identity is null)
                {
                    return Results.NotFound("");
                }

                identity.Name = updateIdentityDto.Name;
                identity.Sinner = updateIdentityDto.Sinner;
                identity.Health = updateIdentityDto.Health;
                identity.DefenseLevel = updateIdentityDto.DefenseLevel;
                identity.MinSpeed = updateIdentityDto.MinSpeed;
                identity.MaxSpeed = updateIdentityDto.MaxSpeed;
                identity.Image = updateIdentityDto.Image;

                if(updateIdentityDto.PassiveIds is not null)
                {
                    var passives = await dbContext.Passives
                    .Where(p => updateIdentityDto.PassiveIds.Contains(p.Id))
                    .ToListAsync();

                    identity.Passives = passives;
                }

                if(updateIdentityDto.SkillIds is not null)
                {
                    var skills = await dbContext.Skills
                    .Where(s => updateIdentityDto.SkillIds.Contains(s.Id))
                    .ToListAsync();

                    identity.Skills = skills;
                }

                dbContext.Update(identity);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();

            });

            group.MapDelete("{id}", async (int id, IdentityDbContext dbContext) =>
            {
                var identity = await dbContext.Identities.FindAsync(id);
                if(identity is not null)
                {
                    await dbContext.Identities.Where(identity => identity.Id == id)
                    .ExecuteDeleteAsync();
                }
                return Results.NoContent();
            });

            return group;
        }
    }
}
