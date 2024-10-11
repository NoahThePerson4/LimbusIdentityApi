using FluentValidation;
using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;
using LimbusIdentityApi.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LimbusIdentityApi.Endpoints
{
    public static class PassiveEndpoints
    {
        public const string GetPassive = "GetPassive";
        public static RouteGroupBuilder MapPassiveEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/Passives")
                .WithTags("Passives");

            group.MapGet("", async (IdentityDbContext dbContext, [AsParameters] GetPassiveDto passiveDto, ILoggerFactory loggerFactory) =>{
                var logger = loggerFactory.CreateLogger(nameof(PassiveEndpoints));
                var passives = new List<Passive>();
                if (passiveDto.filter is not null)
                {
                    bool supportFilter;
                    bool filterIsBool = bool.TryParse(passiveDto.filter, out supportFilter);

                    passives = await dbContext.Passives.Where(passive => passive.Name.Contains(passiveDto.filter) || passive.Description.Contains(passiveDto.filter) || passive.Cost.Contains(passiveDto.filter) || (filterIsBool && passive.Support == supportFilter))
                    .OrderBy(passive => passive.Id)
                    .Skip((passiveDto.pageNumber - 1) * passiveDto.pageSize)
                    .Take(passiveDto.pageSize)
                    .ToListAsync();
                }
                else
                {
                    passives = await dbContext.Passives
                    .OrderBy(passive => passive.Id)
                    .Skip((passiveDto.pageNumber - 1) * passiveDto.pageSize)
                    .Take(passiveDto.pageSize)
                    .ToListAsync();
                }
                var DtoPassives= passives.Select(passive => passive.AsPassiveDto());

                logger.LogInformation("Get On Passives was called for {pages} passives on page {page} with the filter {filter}.", passiveDto.pageSize, passiveDto.pageNumber, passiveDto.filter);
                return Results.Ok(DtoPassives);
            });

            group.MapGet("{id}", async (int id, IdentityDbContext dbContext, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(PassiveEndpoints));
                var passive = await dbContext.Passives.FindAsync(id);
                if( passive is null)
                {
                    logger.LogError("Get was called on Passives with the Id {id} but no Passive with that Id exists!", id);
                    return Results.NotFound("No Passive exists with that Id!");
                }
                logger.LogInformation("Get was called on Passives with the Id {id} for Passive {name}.", id, passive.Name);
                return Results.Ok(passive.AsPassiveDto());
            }).WithName(GetPassive);

            group.MapPost("", async (IdentityDbContext dbContext, CreatePassiveDto passiveDto, ILoggerFactory loggerFactory, IValidator<CreatePassiveDto> createPassiveValidator) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(PassiveEndpoints));
                Passive passive = new()
                {
                    Name = passiveDto.Name,
                    Cost = passiveDto.Cost,
                    Support = passiveDto.Support,
                    Description = passiveDto.Description
                };

                var validationResult = await createPassiveValidator.ValidateAsync(passiveDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                await dbContext.AddAsync(passive);
                await dbContext.SaveChangesAsync();

                logger.LogInformation("New Passive {name} was created with Id {id}.", passive.Name, passive.Id);
                return Results.CreatedAtRoute(GetPassive, new {id = passive.Id}, passive.AsPassiveDto());
            });

            group.MapPut("{id}", async (int id,IdentityDbContext dbContext, CreatePassiveDto updatePassiveDto, ILoggerFactory loggerFactory, IValidator<CreatePassiveDto> updatePassiveValidator) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(PassiveEndpoints));
                var passive = await dbContext.Passives.FindAsync(id);
                if(passive is null)
                {
                    logger.LogError("Put was called for the Passive with the Id of {id} but no Passive with that Id exists!", id);
                    return Results.NotFound("No Passive with that Id exists!");
                }

                var validationResult = await updatePassiveValidator.ValidateAsync(updatePassiveDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                passive.Name = updatePassiveDto.Name;
                passive.Cost = updatePassiveDto.Cost;
                passive.Support = updatePassiveDto.Support;
                passive.Description = updatePassiveDto.Description;

                dbContext.Update(passive);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Passive {name} was updated with an Id of {id}.", passive.Name, id);
                return Results.NoContent();
            });

            group.MapDelete("{id}", async (int id, IdentityDbContext dbContext, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(PassiveEndpoints));
                var passive = await dbContext.Passives.FindAsync(id);

                if(passive is not null)
                {
                    await dbContext.Passives.Where(passive => passive.Id == id)
                    .ExecuteDeleteAsync();
                }
                logger.LogInformation("The Passive with Id {id} was deleted.", id);
                return Results.NoContent();
            });

            return group;
        }
    }
}
