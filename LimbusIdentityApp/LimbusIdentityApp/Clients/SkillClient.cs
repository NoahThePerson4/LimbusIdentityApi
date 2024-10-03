using LimbusIdentityApp.Models;

namespace LimbusIdentityApp.Clients
{
    public class SkillClient(HttpClient httpClient)
    {
        public async Task<SkillSummary[]> GetSkillsAsync(int PageNumber, int PageSize, string? Filter = null)
        {
            if (Filter is null)
            {
                return await httpClient.GetFromJsonAsync<SkillSummary[]>(
                    $"/Skills?pageNumber={PageNumber}&pageSize={PageSize}") ?? [];
            }
            return await httpClient.GetFromJsonAsync<SkillSummary[]>(
                   $"/Skills?filter={Filter}&pageNumber={PageNumber}&pageSize={PageSize}") ?? [];
        }

        public async Task AddSkillAsync(SkillDetails skill)
            => await httpClient.PostAsJsonAsync("/Skills", skill);

        public async Task<SkillDetails> GetSkillAsync(int id)
            => await httpClient.GetFromJsonAsync<SkillDetails>($"/Skills/{id}")
            ?? throw new Exception("Could Not Find Skill!");

        public async Task UpdateSkillAsync(SkillDetails updateSkill)
            => await httpClient.PutAsJsonAsync($"/Skills/{updateSkill.Id}", updateSkill);

        public async Task DeleteSkillAsync(int id)
            => await httpClient.DeleteAsync($"/Skills/{id}");
    }
}
