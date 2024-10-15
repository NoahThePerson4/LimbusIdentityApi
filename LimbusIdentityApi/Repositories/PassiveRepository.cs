using LimbusIdentityApi.Data;
using Microsoft.EntityFrameworkCore;

namespace LimbusIdentityApi.Repositories
{
    public class PassiveRepository : IPassiveRepository
    {
        private readonly IdentityDbContext _dbContext;

        public PassiveRepository(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Passive>> GetAllPassives(string? filter, int pageNumber, int pageSize)
        {
            var skipCount = (pageNumber - 1) * pageSize;

            return await FilterPassive(filter)
                .OrderBy(passive => passive.Id)
                .Skip(skipCount)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Passive?> GetPassive(int id)
        {
            return await _dbContext.Passives.FirstOrDefaultAsync(passive => passive.Id == id);
        }

        public async Task CreatePassive(Passive passive)
        {
            _dbContext.Add(passive);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePassive(Passive updatePassive)
        {
            _dbContext.Update(updatePassive);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePassive(int id)
        {
            await _dbContext.Passives.Where(passive => passive.Id == id)
                .ExecuteDeleteAsync();
        }

        private IQueryable<Passive> FilterPassive(string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return _dbContext.Passives;
            }

            return _dbContext.Passives
                .Where(passive =>
                passive.Name.Contains(filter) || passive.Description.Contains(filter) || passive.Cost.Contains(filter));
        }
    }
}
