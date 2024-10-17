using LimbusIdentityApi.Extensions;
using LimbusIdentityApi.Repositories;

namespace LimbusIdentityApi.Services
{
    public class PassiveService
    {
        private readonly IPassiveRepository _repository;
        private readonly ILogger<PassiveService> _logger;

        public PassiveService(IPassiveRepository repository, ILogger<PassiveService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IResult> GetPassive(int id)
        {
            var passive = await _repository.GetPassive(id);
            if (passive is null)
            {
                _logger.LogError("Get was called on Passives with the Id {id} but no Passive with that Id exists!", id);
                return Results.NotFound("No Passive exists with that Id!");
            }

            _logger.LogInformation("Get was called on Passives with the Id {id} for Passive {name}.", id, passive.Name);
            return Results.Ok(passive.AsPassiveDto());
        }
    }
}
