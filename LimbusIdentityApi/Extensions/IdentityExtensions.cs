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

        public static IdentityDetailsDto AsIdentityDetailedDto(this Identity identity)
        {
            return new IdentityDetailsDto(
                identity.Id,
                identity.Name,
                identity.Sinner,
                identity.Health,
                identity.DefenseLevel,
                identity.MinSpeed,
                identity.MaxSpeed,
                identity.Image,
                identity.Passives.Select(p => p.Id).ToList(),
                identity.Passives.Select(p => p.Name).ToList(),
                identity.Passives.Select(p => p.Cost).ToList(),
                identity.Passives.Select(p => p.Description).ToList(),
                identity.Skills.Select(s => s.Id).ToList(),
                identity.Skills.Select(s => s.Name).ToList(),
                identity.Skills.Select(s=>s.Type).ToList(),
                identity.Skills.Select(s=> s.Sin).ToList(),
                identity.Skills.Select(s => s.MinRoll).ToList(),
                identity.Skills.Select(s => s.MaxRoll).ToList(),
                identity.Skills.Select(s => s.SkillEffect).ToList(),
                identity.Skills.SelectMany(s => s.CoinEffects).ToList()
                );
        }
    }
}
