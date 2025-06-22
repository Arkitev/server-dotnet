using server_dotnet.Dtos;

namespace server_dotnet.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync(int pageNumber, int pageSize);
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(UserCreateUpdateDto dto);
    Task UpdateAsync(int id, UserCreateUpdateDto dto);
    Task DeleteAsync(int id);
}
