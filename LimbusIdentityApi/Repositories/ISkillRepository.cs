using LimbusIdentityApi.Data;

namespace LimbusIdentityApi.Repositories
{
    public interface ISkillRepository
    {
        public Task<IEnumerable<Skill>> GetAllSkills(string? filter, int pageNumber, int pageSize);
        public Task<Skill?> GetSkill(int id);
        public Task CreateSkill(Skill skill);
        public Task UpdateSkill(Skill updateSkill);
        public Task DeleteSkill(int id);
    }
}
