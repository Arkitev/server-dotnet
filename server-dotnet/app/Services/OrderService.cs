using AutoMapper;
using server_dotnet.Dtos;
using server_dotnet.Services.Interfaces;
using server_dotnet_dal.Entities;
using server_dotnet_dal.Repositories.Interfaces;

namespace server_dotnet.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IOrganizationRepository organizationRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        var orders = await _orderRepository.GetAllAsync(pageNumber, pageSize);

        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        return order == null ? null : _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> CreateAsync(OrderCreateUpdateDto dto)
    {
        if (dto.TotalAmount <= 0)
            throw new ArgumentException("TotalAmount must be greater than 0.");

        _ = await _userRepository.GetByIdAsync(dto.UserId)
            ?? throw new ArgumentException("User does not exist.");

        _ = await _organizationRepository.GetByIdAsync(dto.OrganizationId)
            ?? throw new ArgumentException("Organization does not exist.");

        var entity = _mapper.Map<Order>(dto);
        entity.OrderDate = DateTime.UtcNow;
        var created = await _orderRepository.AddAsync(entity);

        return _mapper.Map<OrderDto>(created);
    }

    public async Task UpdateAsync(int id, OrderCreateUpdateDto dto)
    {
        if (dto.TotalAmount <= 0)
            throw new ArgumentException("TotalAmount must be greater than 0.");

        var existing = await _orderRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Order not found.");

        _ = await _userRepository.GetByIdAsync(dto.UserId)
            ?? throw new ArgumentException("User does not exist.");

        _ = await _organizationRepository.GetByIdAsync(dto.OrganizationId)
            ?? throw new ArgumentException("Organization does not exist.");

        _mapper.Map(dto, existing);

        await _orderRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _orderRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Order not found.");

        await _orderRepository.DeleteAsync(existing);
    }
}
