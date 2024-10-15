using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;

namespace LimbusIdentityApi.Repositories
{
    public interface IIdentityRepository
    {
        public Task<IEnumerable<Identity>> GetAllIdentities(string? filter, int pageNumber, int pageSize);
        public Task<Identity?> GetIdentity(int id);
        public Task<Identity> CreateIdentity(CreateIdentityDto identityDto);
        public Task<Identity?> UpdateIdentity(int id, CreateIdentityDto updateIdentityDto);
        public Task DeleteIdentity(int id);
    }
}
