namespace LimbusIdentityApi.Dtos
{
    public record CreatePassiveDto(
        string Name,
        string Cost,
        bool Support,
        string Description);
}
