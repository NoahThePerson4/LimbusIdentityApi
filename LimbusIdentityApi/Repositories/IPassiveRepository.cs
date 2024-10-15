using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;

namespace LimbusIdentityApi.Repositories
{
    public interface IPassiveRepository
    {
        public Task<IEnumerable<Passive>> GetAllPassives(string? filter, int pageNumber, int pageSize);
        public Task<Passive?> GetPassive(int id);
        public Task CreatePassive(Passive passive);
        public Task UpdatePassive(Passive updatePassive);
        public Task DeletePassive(int id);
    }
}
