using LimbusIdentityApi.Data;
using Microsoft.EntityFrameworkCore;

namespace LimbusIdentityApi.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly IdentityDbContext _dbContext;

        public SkillRepository(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Skill>> GetAllSkills(string? filter, int pageNumber, int pageSize)
        {
            var skipCount = (pageNumber - 1) * pageSize;

            return await FilterSkills(filter)
                .OrderBy(skill => skill.Id)
                .Skip(skipCount)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Skill?> GetSkill(int id)
        {
            return await _dbContext.Skills.FirstOrDefaultAsync(skill => skill.Id == id);
        }
        public async Task CreateSkill(Skill skill)
        {
            _dbContext.Add(skill);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateSkill(Skill updateSkill)
        {
            _dbContext.Update(updateSkill);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSkill(int id)
        {
            var skill = await _dbContext.Skills.FindAsync(id);

            if (skill is not null)
            {
                _dbContext.Skills.Remove(skill);
                await _dbContext.SaveChangesAsync();
            }

        }

        private IQueryable<Skill> FilterSkills(string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return _dbContext.Skills;
            }

            return _dbContext.Skills
                .Where(skill =>
                skill.Name.Contains(filter) || skill.Type.Contains(filter) || skill.Sin.Contains(filter));
        }
    }
}
