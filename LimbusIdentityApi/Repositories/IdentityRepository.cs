using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;
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
            return await _dbContext.Identities
                .Include(e => e.Skills)
                .Include(e => e.Passives)
                .FirstOrDefaultAsync(ids => ids.Id == id);     
        }

        public async Task<Identity> CreateIdentity(CreateIdentityDto identityDto)
        {
            Identity identity = new()
            {
                Name = identityDto.Name,
                Sinner = identityDto.Sinner,
                Health = identityDto.Health,
                Ineffective = identityDto.Ineffective,
                Fatal = identityDto.Fatal,
                DefenseLevel = identityDto.DefenseLevel,
                MinSpeed = identityDto.MinSpeed,
                MaxSpeed = identityDto.MaxSpeed,
                Image = identityDto.Image
            };

            if (identityDto.PassiveIds is not null)
            {
                var passives = await _dbContext.Passives
                    .Where(p => identityDto.PassiveIds.Contains(p.Id))
                    .ToListAsync();

                identity.Passives = passives;
            }

            if (identityDto.SkillIds is not null)
            {
                var skills = await _dbContext.Skills
                    .Where(s => identityDto.SkillIds.Contains(s.Id))
                    .ToListAsync();

                identity.Skills = skills;
            }

            await _dbContext.AddAsync(identity);
            await _dbContext.SaveChangesAsync();
            return identity;
        }

        public async Task<Identity?> UpdateIdentity(int id, CreateIdentityDto updateIdentityDto)
        {
            var identity = await _dbContext.Identities
          .Include(i => i.Passives)
          .Include(i => i.Skills)
          .FirstOrDefaultAsync(i => i.Id == id);

            if (identity == null)
            {
                return null;
            }

            identity.Name = updateIdentityDto.Name;
            identity.Sinner = updateIdentityDto.Sinner;
            identity.Health = updateIdentityDto.Health;
            identity.Ineffective = updateIdentityDto.Ineffective;
            identity.Fatal = updateIdentityDto.Fatal;
            identity.DefenseLevel = updateIdentityDto.DefenseLevel;
            identity.MinSpeed = updateIdentityDto.MinSpeed;
            identity.MaxSpeed = updateIdentityDto.MaxSpeed;
            identity.Image = updateIdentityDto.Image;

            if (updateIdentityDto.PassiveIds != null)
            {
                var passives = await _dbContext.Passives
                    .Where(p => updateIdentityDto.PassiveIds.Contains(p.Id))
                    .ToListAsync();
                identity.Passives = passives;
            }

            if (updateIdentityDto.SkillIds != null)
            {
                var skills = await _dbContext.Skills
                    .Where(s => updateIdentityDto.SkillIds.Contains(s.Id))
                    .ToListAsync();
                identity.Skills = skills;
            }

            _dbContext.Update(identity);
            await _dbContext.SaveChangesAsync();

            return identity;
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
