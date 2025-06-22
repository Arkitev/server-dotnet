using server_dotnet.Dtos;

namespace server_dotnet.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync(int pageNumber, int pageSize);
    Task<OrderDto?> GetByIdAsync(int id);
    Task<OrderDto> CreateAsync(OrderCreateUpdateDto dto);
    Task UpdateAsync(int id, OrderCreateUpdateDto dto);
    Task DeleteAsync(int id);
}
