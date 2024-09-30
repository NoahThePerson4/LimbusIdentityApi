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

            group.MapGet("", async (IdentityDbContext dbContext)=>{
                var passives = (await dbContext.Passives.ToListAsync());
                var DtoPassives= passives.Select(passive => passive.AsPassiveDto());
                return Results.Ok(DtoPassives);
            });

            group.MapGet("{id}", async (int id, IdentityDbContext dbContext) =>
            {
                var passive = await dbContext.Passives.FindAsync(id);
                if( passive is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(passive.AsPassiveDto());
            }).WithName(GetPassive);

            group.MapPost("", async (IdentityDbContext dbContext, CreatePassiveDto passiveDto) =>
            {
                Passive passive = new()
                {
                    Name = passiveDto.Name,
                    Cost = passiveDto.Cost,
                    Description = passiveDto.Description
                };

                await dbContext.AddAsync(passive);
                await dbContext.SaveChangesAsync();

                return Results.CreatedAtRoute(GetPassive, new {id = passive.Id}, passive.AsPassiveDto());
            });

            group.MapPut("{id}", async (int id,IdentityDbContext dbContext, CreatePassiveDto updatePassiveDto) =>
            {
                var passive = await dbContext.Passives.FindAsync(id);
                if(passive is null)
                {
                    return Results.NotFound();
                }

                passive.Name = updatePassiveDto.Name;
                passive.Cost = updatePassiveDto.Cost;
                passive.Description = updatePassiveDto.Description;

                dbContext.Update(passive);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            });

            group.MapDelete("{id}", async (int id, IdentityDbContext dbContext) =>
            {
                var passive = await dbContext.Passives.FindAsync(id);

                if(passive is not null)
                {
                    await dbContext.Passives.Where(passive => passive.Id == id)
                    .ExecuteDeleteAsync();
                }
                return Results.NoContent();
            });

            return group;
        }
    }
}
