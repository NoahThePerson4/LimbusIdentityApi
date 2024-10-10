using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;

namespace LimbusIdentityApi.Extensions
{
    public static class PassiveExtensions
    {
        public static PassiveDto AsPassiveDto(this Passive passive)
        {
            return new PassiveDto(
                passive.Id,
                passive.Name,
                passive.Cost,
                passive.Support,
                passive.Description);
        }
    }
}
