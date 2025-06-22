using server_dotnet_dal.Entities;

namespace server_dotnet_dal.Repositories.Interfaces;

public interface IOrganizationRepository
{
    Task<IEnumerable<Organization>> GetAllAsync(int pageNumber, int pageSize);
    Task<Organization?> GetByIdAsync(int id);
    Task<Organization> AddAsync(Organization organization);
    Task UpdateAsync(Organization organization);
    Task DeleteAsync(Organization organization);
}
