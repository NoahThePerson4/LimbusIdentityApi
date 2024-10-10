namespace LimbusIdentityApi.Dtos
{
    public record PassiveDto(
        int Id,
        string Name,
        string? Cost,
        bool? Support,
        string Description);
    
    
}
