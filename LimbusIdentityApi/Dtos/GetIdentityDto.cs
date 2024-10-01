namespace LimbusIdentityApi.Dtos
{
    public record GetIdentityDto(
        string? filter,
        int pageNumber = 1,
        int pageSize = 10);
}
