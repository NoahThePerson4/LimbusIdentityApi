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

            group.MapGet("", async (IdentityDbContext dbContext, [AsParameters] GetIdentityDto identityDto, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(IdentityEndpoints));

                var identities = new List<Identity>();

                if(identityDto.filter is not null)
                {
                    identities = await dbContext.Identities.Where(identity => identity.Name.Contains(identityDto.filter) || identity.Sinner.Contains(identityDto.filter))
                    .OrderBy(i => i.Id)
                    .Skip((identityDto.pageNumber - 1) * identityDto.pageSize)
                    .Take(identityDto.pageSize)
                    .Include(identity => identity.Passives)
                    .Include(identity => identity.Skills)
                    .ToListAsync();
                }
                else
                {
                    identities = await dbContext.Identities
                    .OrderBy(i => i.Id)
                    .Skip((identityDto.pageNumber - 1) * identityDto.pageSize)
                    .Take(identityDto.pageSize)
                    .Include(identity => identity.Passives)
                    .Include(identity => identity.Skills)
                    .ToListAsync();
                }

                var identitiesDtos = identities.Select(identity => identity.AsIdentityDto());
                logger.LogInformation("Get on Identities was called for {pages} identities on page {page} with the filter {filter}.", identityDto.pageSize, identityDto.pageNumber, identityDto.filter);
                return Results.Ok(identitiesDtos);
            });

            group.MapGet("{id}", async (int id, IdentityDbContext dbContext, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(IdentityEndpoints));
                var identity = await dbContext.Identities
                .Include(identity => identity.Passives)
                .Include(identity => identity.Skills)
                .SingleOrDefaultAsync(identity => identity.Id == id);

                if (identity is null)
                {
                    logger.LogError("Get on Identities was called for the Id {id} but no Identity with that Id exists!", id);
                    return Results.NotFound("No Identity with that Id exists!");
                }
                logger.LogInformation("Get on Identities was called for the Identity {name} {sinner} with Id {id}.", identity.Name,identity.Sinner, id);
                return Results.Ok(identity.AsIdentityDetailedDto());
            })
                .WithName(GetIdentity);

            group.MapPost("", async (IdentityDbContext dbContext, CreateIdentityDto identityDto, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(IdentityEndpoints));
                Identity identity = new()
                {
                    Name = identityDto.Name,
                    Sinner = identityDto.Sinner,
                    Health = identityDto.Health,
                    Ineffective = identityDto.Ineffective,
                    Fatal = identityDto.Fatal,
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
                
                if(identityDto.SkillIds is null )
                {
                    logger.LogError("No Skills were given to the Identity {name} {sinner}.", identity.Name, identity.Sinner);
                } 
                if(identityDto.PassiveIds is null)
                {
                    logger.LogError("No Passives were given to the Identity {name} {sinner}.", identity.Name, identity.Sinner);
                }

                await dbContext.AddAsync(identity);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("Created new Identity {name} {sinner} with the Id {id}.", identity.Name, identity.Sinner, identity.Id);
                return Results.CreatedAtRoute(GetIdentity, new { id = identity.Id }, identity.AsIdentityDto());
            });

            group.MapPut("{id}", async (int id, IdentityDbContext dbContext, CreateIdentityDto updateIdentityDto, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(IdentityEndpoints));
                var identity = await dbContext.Identities.FindAsync(id);

                if(identity is null)
                {
                    logger.LogError("Put was called on Identities with the Id {id} but no Identity with that Id exists!", id);
                    return Results.NotFound("No Identity with that Id exists!");
                }

                identity.Name = updateIdentityDto.Name;
                identity.Sinner = updateIdentityDto.Sinner;
                identity.Health = updateIdentityDto.Health;
                identity.Ineffective = updateIdentityDto.Ineffective;
                identity.Fatal = updateIdentityDto.Fatal;
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
                logger.LogInformation("The Identity {name} {sinner} was updated with Id {id}.", identity.Name, identity.Sinner, id);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();

            });

            group.MapDelete("{id}", async (int id, IdentityDbContext dbContext, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(IdentityEndpoints));
                var identity = await dbContext.Identities.FindAsync(id);
                if(identity is not null)
                {
                    await dbContext.Identities.Where(identity => identity.Id == id)
                    .ExecuteDeleteAsync();
                }
                logger.LogInformation("Delete was called for the Identity with Id {id}.", id);
                return Results.NoContent();
            });

            return group;
        }
    }
}
