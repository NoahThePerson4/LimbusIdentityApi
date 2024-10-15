using LimbusIdentityApi.Data;
using Microsoft.EntityFrameworkCore;

namespace LimbusIdentityApi.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IdentityDbContext _dbContext;

        public IdentityRepository(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Identity>> GetAllIdentities(string? filter, int pageNumber, int pageSize)
        {
            var skipCount = (pageNumber - 1) * pageSize;

            return await FilterIdentities(filter)
                .OrderBy(ids => ids.Id)
                .Skip(skipCount)
                .Include(e => e.Skills)
                .Include(e => e.Passives)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Identity?> GetIdentity(int id)
        {
            return await _dbContext.Identities.FirstOrDefaultAsync(ids => ids.Id == id);
        }

        public async Task CreateIdentity(Identity identity)
        {
            _dbContext.Add(identity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateIdentity(Identity updateIdentity)
        {
            _dbContext.Update(updateIdentity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteIdentity(int id)
        {
            await _dbContext.Identities.Where(ids => ids.Id == id)
                .ExecuteDeleteAsync();
        }
        private IQueryable<Identity> FilterIdentities(string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return _dbContext.Identities;
            }

            return _dbContext.Identities
                .Where(identities =>
                identities.Name.Contains(filter));
        }
    }
}
