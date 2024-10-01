namespace LimbusIdentityApi.Dtos
{
    public record GetSkillDto(
        string? filter,
        int pageNumber = 1,
        int pageSize = 10);   
}
