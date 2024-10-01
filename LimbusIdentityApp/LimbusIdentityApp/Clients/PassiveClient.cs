using LimbusIdentityApp.Models;

namespace LimbusIdentityApp.Clients
{
    public class PassiveClient(HttpClient httpClient)
    {
        public async Task<PassiveSummary[]> GetPassivesAsync(int PageNumber, int PageSize, string? Filter = null)
        {
            if (Filter is null)
            {
                return await httpClient.GetFromJsonAsync<PassiveSummary[]>(
                    $"/Passives?PageNumber={PageNumber}&PageSize={PageSize}") ?? [];
            }
            return await httpClient.GetFromJsonAsync<PassiveSummary[]>(
                   $"/Passives?Filter={Filter}&PageNumber={PageNumber}&PageSize={PageSize}") ?? [];
        }

        public async Task AddPassiveAsync(PassiveDetails passive)
            => await httpClient.PostAsJsonAsync("/Passives", passive);

        public async Task<PassiveDetails> GetPassiveAsync(int id)
            => await httpClient.GetFromJsonAsync<PassiveDetails>($"/Passives/{id}")
            ?? throw new Exception("Could Not Find Passive!");

        public async Task UpdatePassiveAsync(PassiveDetails updatePassive)
            => await httpClient.PutAsJsonAsync($"/Passives/{updatePassive.Id}", updatePassive);

        public async Task DeletePassiveAsync(int id)
            => await httpClient.DeleteAsync($"/Passives/{id}");
    }
}
