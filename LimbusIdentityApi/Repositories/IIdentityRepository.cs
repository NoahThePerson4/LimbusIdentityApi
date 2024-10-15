using LimbusIdentityApi.Data;

namespace LimbusIdentityApi.Repositories
{
    public interface IIdentityRepository
    {
        public Task<IEnumerable<Identity>> GetAllIdentities(string? filter, int pageNumber, int pageSize);
        public Task<Identity?> GetIdentity(int id);
        public Task CreateIdentity(Identity identity);
        public Task UpdateIdentity(Identity updateIdentity);
        public Task DeleteIdentity(int id);
    }
}
