using server_dotnet.Dtos;

namespace server_dotnet.Services.Interfaces;

public interface IOrganizationService
{
    Task<IEnumerable<OrganizationDto>> GetAllAsync(int pageNumber, int pageSize);
    Task<OrganizationDto?> GetByIdAsync(int id);
    Task<OrganizationDto> CreateAsync(OrganizationCreateUpdateDto dto);
    Task UpdateAsync(int id, OrganizationCreateUpdateDto dto);
    Task DeleteAsync(int id);
}
