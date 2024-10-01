using LimbusIdentityApi.Data;
using LimbusIdentityApi.Dtos;

namespace LimbusIdentityApi.Extensions
{
    public static class IdentityExtensions
    {
        public static IdentityDto AsIdentityDto(this Identity identity)
        {
            List<string> passives = ["None"];
            List<string> skills = ["None"];
            if(identity.Passives is not null)
            {
                passives = identity.Passives.Select(p => p.Name).ToList();
            }

            if(identity.Skills is not null)
            {
                skills = identity.Skills.Select(s => s.Name).ToList();
            }

            return new IdentityDto(
                identity.Id,
                identity.Name,
                identity.Sinner,
                identity.Health,
                identity.DefenseLevel,
                identity.MinSpeed,
                identity.MaxSpeed,
                identity.Image,
                passives,
                skills
                );
        }
    }
}
