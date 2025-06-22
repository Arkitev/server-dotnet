using AutoMapper;
using server_dotnet.Dtos;
using server_dotnet.Services.Interfaces;
using server_dotnet_dal.Entities;
using server_dotnet_dal.Repositories.Interfaces;

namespace server_dotnet.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public OrganizationService(IOrganizationRepository orgRepository, IMapper mapper)
    {
        _organizationRepository = orgRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrganizationDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        var orgs = await _organizationRepository.GetAllAsync(pageNumber, pageSize);

        return _mapper.Map<IEnumerable<OrganizationDto>>(orgs);
    }

    public async Task<OrganizationDto?> GetByIdAsync(int id)
    {
        var org = await _organizationRepository.GetByIdAsync(id);

        return org == null ? null : _mapper.Map<OrganizationDto>(org);
    }

    public async Task<OrganizationDto> CreateAsync(OrganizationCreateUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Organization name is required.");

        var entity = _mapper.Map<Organization>(dto);
        entity.DateFounded = DateTime.UtcNow;
        var created = await _organizationRepository.AddAsync(entity);

        return _mapper.Map<OrganizationDto>(created);
    }

    public async Task UpdateAsync(int id, OrganizationCreateUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Organization name is required.");

        var existing = await _organizationRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Organization not found.");

        _mapper.Map(dto, existing);

        await _organizationRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _organizationRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Organization not found.");

        await _organizationRepository.DeleteAsync(existing);
    }
}
