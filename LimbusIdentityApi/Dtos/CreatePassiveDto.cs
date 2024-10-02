namespace LimbusIdentityApi.Dtos
{
    public record CreatePassiveDto(
        string Name,
        string? Cost,
        string Description);
}
