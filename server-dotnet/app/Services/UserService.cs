using AutoMapper;
using server_dotnet.Dtos;
using server_dotnet.Services.Interfaces;
using server_dotnet_dal.Entities;
using server_dotnet_dal.Repositories.Interfaces;

namespace server_dotnet.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        var users = await _userRepository.GetAllAsync(pageNumber, pageSize);

        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(UserCreateUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
            throw new ArgumentException("First and last name are required.");

        _ = await _organizationRepository.GetByIdAsync(dto.OrganizationId)
            ?? throw new KeyNotFoundException("Organization not found.");

        var entity = _mapper.Map<User>(dto);
        entity.DateCreated = DateTime.UtcNow;
        var created = await _userRepository.AddAsync(entity);

        return _mapper.Map<UserDto>(created);
    }

    public async Task UpdateAsync(int id, UserCreateUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
            throw new ArgumentException("First and last name are required.");

        var existing = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("User not found.");

        _ = await _organizationRepository.GetByIdAsync(dto.OrganizationId)
            ?? throw new KeyNotFoundException("Organization not found.");

        _mapper.Map(dto, existing);

        await _userRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("User not found.");

        await _userRepository.DeleteAsync(existing);
    }
}
