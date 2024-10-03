using LimbusIdentityApp.Models;

namespace LimbusIdentityApp.Clients
{
    public class IdentityClient (HttpClient httpClient)
    {
        public async Task<IdentitySummary[]> GetIdentitiesAsync(int PageNumber, int PageSize, string? Filter = null)
        {
            if (Filter is null)
            {
                return await httpClient.GetFromJsonAsync<IdentitySummary[]>(
                    $"/Identities?pageNumber={PageNumber}&pageSize={PageSize}") ?? [];
            }
            return await httpClient.GetFromJsonAsync<IdentitySummary[]>(
                   $"/Identities?filter={Filter}&pageNumber={PageNumber}&pageSize={PageSize}") ?? [];
        }

        public async Task AddIdentityAsync(IdentityDetails Identity)
            => await httpClient.PostAsJsonAsync("/Identities", Identity);

        public async Task<IdentityDetails> GetIdentityAsync(int id)
            => await httpClient.GetFromJsonAsync<IdentityDetails>($"/Identities/{id}")
            ?? throw new Exception("Could Not Find Identity!");

        public async Task UpdateIdentityAsync(IdentityDetails updateIdentity)
            => await httpClient.PutAsJsonAsync($"/Identities/{updateIdentity.Id}", updateIdentity);

        public async Task DeleteIdentityAsync(int id)
            => await httpClient.DeleteAsync($"/Identities/{id}");
    }
}
