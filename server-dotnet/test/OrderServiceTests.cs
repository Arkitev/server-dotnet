using AutoMapper;
using FluentAssertions;
using Moq;
using server_dotnet.Dtos;
using server_dotnet.Services;
using server_dotnet_dal.Entities;
using server_dotnet_dal.Repositories.Interfaces;

namespace server_dotnet.tests;

public class OrderServiceBusinessLogicTests
{
    private readonly Mock<IOrderRepository> _orderRepoMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IOrganizationRepository> _orgRepoMock = new();
    private readonly IMapper _mapper;
    private readonly OrderService _service;

    public OrderServiceBusinessLogicTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Order, OrderDto>();
            cfg.CreateMap<OrderCreateUpdateDto, Order>();
        });
        _mapper = config.CreateMapper();

        _service = new OrderService(_orderRepoMock.Object, _userRepoMock.Object, _orgRepoMock.Object, _mapper);
    }

    [Fact]
    public async Task CreateAsync_Throws_When_TotalAmountIsZeroOrLess()
    {
        var dto = new OrderCreateUpdateDto { TotalAmount = 0, UserId = 1, OrganizationId = 1 };

        await _service.Invoking(s => s.CreateAsync(dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("TotalAmount must be greater than 0.");
    }

    [Fact]
    public async Task CreateAsync_Throws_When_UserDoesNotExist()
    {
        var dto = new OrderCreateUpdateDto { TotalAmount = 10, UserId = 1, OrganizationId = 1 };
        _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId)).ReturnsAsync((User?)null);

        await _service.Invoking(s => s.CreateAsync(dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("User does not exist.");
    }

    [Fact]
    public async Task CreateAsync_Throws_When_OrganizationDoesNotExist()
    {
        var dto = new OrderCreateUpdateDto { TotalAmount = 10, UserId = 1, OrganizationId = 1 };
        _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId)).ReturnsAsync(new User());
        _orgRepoMock.Setup(r => r.GetByIdAsync(dto.OrganizationId)).ReturnsAsync((Organization?)null);

        await _service.Invoking(s => s.CreateAsync(dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Organization does not exist.");
    }

    [Fact]
    public async Task CreateAsync_Returns_OrderDto_When_Successful()
    {
        // Arrange
        var dto = new OrderCreateUpdateDto
        {
            TotalAmount = 100,
            UserId = 1,
            OrganizationId = 1
        };

        var user = new User { Id = dto.UserId };
        var organization = new Organization { Id = dto.OrganizationId };
        var orderEntity = new Order
        {
            Id = 123,
            TotalAmount = dto.TotalAmount,
            UserId = dto.UserId,
            OrganizationId = dto.OrganizationId,
            OrderDate = DateTime.UtcNow
        };

        _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId)).ReturnsAsync(user);
        _orgRepoMock.Setup(r => r.GetByIdAsync(dto.OrganizationId)).ReturnsAsync(organization);
        _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>())).ReturnsAsync(orderEntity);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(orderEntity.Id);
        result.TotalAmount.Should().Be(dto.TotalAmount);
        result.UserId.Should().Be(dto.UserId);
        result.OrganizationId.Should().Be(dto.OrganizationId);

        _userRepoMock.Verify(r => r.GetByIdAsync(dto.UserId), Times.Once);
        _orgRepoMock.Verify(r => r.GetByIdAsync(dto.OrganizationId), Times.Once);
        _orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_TotalAmountIsZeroOrLess()
    {
        var dto = new OrderCreateUpdateDto { TotalAmount = 0, UserId = 1, OrganizationId = 1 };

        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("TotalAmount must be greater than 0.");
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_OrderNotFound()
    {
        var dto = new OrderCreateUpdateDto { TotalAmount = 10, UserId = 1, OrganizationId = 1 };
        _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Order?)null);

        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Order not found.");
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_UserNotFound()
    {
        var dto = new OrderCreateUpdateDto { TotalAmount = 10, UserId = 1, OrganizationId = 1 };
        _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Order());
        _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId)).ReturnsAsync((User?)null);

        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("User does not exist.");
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_OrganizationNotFound()
    {
        var dto = new OrderCreateUpdateDto { TotalAmount = 10, UserId = 1, OrganizationId = 1 };
        _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Order());
        _userRepoMock.Setup(r => r.GetByIdAsync(dto.UserId)).ReturnsAsync(new User());
        _orgRepoMock.Setup(r => r.GetByIdAsync(dto.OrganizationId)).ReturnsAsync((Organization?)null);

        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Organization does not exist.");
    }

    [Fact]
    public async Task DeleteAsync_Throws_When_OrderNotFound()
    {
        _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Order?)null);

        await _service.Invoking(s => s.DeleteAsync(1))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Order not found.");
    }
}
