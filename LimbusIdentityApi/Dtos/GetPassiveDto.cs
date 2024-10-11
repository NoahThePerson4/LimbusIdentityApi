namespace LimbusIdentityApi.Dtos
{
    public record GetPassiveDto(
        string? filter,
        int pageNumber = 1,
        int pageSize = 10);
}
