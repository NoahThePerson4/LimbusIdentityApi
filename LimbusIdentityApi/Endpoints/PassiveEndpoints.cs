using FluentValidation;
using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;
using LimbusIdentityApi.Extensions;
using LimbusIdentityApi.Repositories;
using LimbusIdentityApi.Services;
using Microsoft.EntityFrameworkCore;

namespace LimbusIdentityApi.Endpoints
{
    public static class PassiveEndpoints
    {
        public const string getPassive = "GetPassive";
        public static RouteGroupBuilder MapPassiveEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/Passives")
                .WithTags("Passives");

            group.MapGet("", async (IPassiveRepository repository, [AsParameters] GetPassiveDto request, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(PassiveEndpoints));
                var passives = (await repository.GetAllPassives(request.filter, request.pageNumber, request.pageSize)).Select(passive => passive.AsPassiveDto());

                logger.LogInformation("Get On Passives was called for {pages} passives on page {page} with the filter {filter}.", request.pageSize, request.pageNumber, request.filter);
                return Results.Ok(passives);
            });

            group.MapGet("{id}", async (int id, IPassiveRepository repository, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(PassiveEndpoints));
                var passive = await repository.GetPassive(id);
                if (passive is null)
                {
                    logger.LogError("Get was called on Passives with the Id {id} but no Passive with that Id exists!", id);
                    return Results.NotFound("No Passive exists with that Id!");
                }
                logger.LogInformation("Get was called on Passives with the Id {id} for Passive {name}.", id, passive.Name);
                return Results.Ok(passive.AsPassiveDto());
            }).WithName(getPassive);

            group.MapPost("", async (IPassiveRepository repository, CreatePassiveDto passiveDto, ILoggerFactory loggerFactory, IValidator<CreatePassiveDto> createPassiveValidator) =>
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

                await repository.CreatePassive(passive);

                logger.LogInformation("New Passive {name} was created with Id {id}.", passive.Name, passive.Id);
                return Results.CreatedAtRoute(getPassive, new { id = passive.Id }, passive.AsPassiveDto());
            });

            group.MapPut("{id}", async (int id, IPassiveRepository repository, CreatePassiveDto updatePassiveDto, ILoggerFactory loggerFactory, IValidator<CreatePassiveDto> updatePassiveValidator) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(PassiveEndpoints));
                var passive = await repository.GetPassive(id);
                if (passive is null)
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

                await repository.UpdatePassive(passive);
                logger.LogInformation("Passive {name} was updated with an Id of {id}.", passive.Name, id);
                return Results.NoContent();
            });

            group.MapDelete("{id}", async (int id, IPassiveRepository repository, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(PassiveEndpoints));
                var passive = await repository.GetPassive(id);

                if (passive is not null)
                {
                    try
                    {
                        await repository.DeletePassive(id);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while trying to delete the Passive with Id {id}: {message}", id, ex.Message);
                        return Results.Problem("An error occurred while deleting the passive. Please try again later.");
                    }
                }
                logger.LogInformation("The Passive with Id {id} was deleted.", id);
                return Results.NoContent();
            });

            return group;
        }
    }
}
