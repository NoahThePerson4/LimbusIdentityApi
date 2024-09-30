namespace LimbusIdentityApi.Dtos
{
    public record IdentityDto(
        string Name,
        string Sinner,
        int Health,
        int DefenseLevel,
        int MinSpeed,
        int MaxSpeed,
        string? Image
        );
    
    
}
