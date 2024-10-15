using FluentValidation;
using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;
using LimbusIdentityApi.Extensions;
using LimbusIdentityApi.Repositories;
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

            group.MapGet("", async (IIdentityRepository repository, [AsParameters] GetIdentityDto request, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(IdentityEndpoints));

                var identities = (await repository.GetAllIdentities(request.filter, request.pageNumber, request.pageSize)).Select(ids => ids.AsIdentityDto());

                logger.LogInformation("Get on Identities was called for {pages} identities on page {page} with the filter {filter}.", request.pageSize, request.pageNumber, request.filter);
                return Results.Ok(identities);
            });

            group.MapGet("{id}", async (int id, IIdentityRepository repository, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(IdentityEndpoints));
                var identity = await repository.GetIdentity(id);

                if (identity is null)
                {
                    logger.LogError("Get on Identities was called for the Id {id} but no Identity with that Id exists!", id);
                    return Results.NotFound("No Identity with that Id exists!");
                }
                logger.LogInformation("Get on Identities was called for the Identity {name} {sinner} with Id {id}.", identity.Name,identity.Sinner, id);
                return Results.Ok(identity.AsIdentityDetailedDto());
            })
                .WithName(GetIdentity);

            group.MapPost("", async (IIdentityRepository repository, CreateIdentityDto identityDto, ILoggerFactory loggerFactory, IValidator<CreateIdentityDto> createIdentityValidator) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(IdentityEndpoints));
                
                
                var validationResult = await createIdentityValidator.ValidateAsync(identityDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var identity = await repository.CreateIdentity(identityDto);

                logger.LogInformation("Created new Identity {name} {sinner} with the Id {id}.", identity.Name, identity.Sinner, identity.Id);
                return Results.CreatedAtRoute(GetIdentity, new { id = identity.Id }, identity.AsIdentityDto());
            });

            group.MapPut("{id}", async (int id, IIdentityRepository repository, CreateIdentityDto updateIdentityDto, ILoggerFactory loggerFactory, IValidator<CreateIdentityDto> updatedIdentityValidator) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(IdentityEndpoints));
                var identity = await repository.GetIdentity(id);

                if(identity is null)
                {
                    logger.LogError("Put was called on Identities with the Id {id} but no Identity with that Id exists!", id);
                    return Results.NotFound("No Identity with that Id exists!");
                }

                var validationResult = await updatedIdentityValidator.ValidateAsync(updateIdentityDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                await repository.UpdateIdentity(id, updateIdentityDto);
                logger.LogInformation("The Identity {name} {sinner} was updated with Id {id}.", identity.Name, identity.Sinner, id);

                return Results.NoContent();

            });

            group.MapDelete("{id}", async (int id, IIdentityRepository repository, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(nameof(IdentityEndpoints));
                var identity = await repository.GetIdentity(id);
                if(identity is not null)
                {
                    await repository.DeleteIdentity(id);
                }
                logger.LogInformation("Delete was called for the Identity with Id {id}.", id);
                return Results.NoContent();
            });

            return group;
        }
    }
}
