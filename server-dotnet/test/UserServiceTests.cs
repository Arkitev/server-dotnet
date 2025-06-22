using AutoMapper;
using FluentAssertions;
using Moq;
using server_dotnet.Dtos;
using server_dotnet.Services;
using server_dotnet_dal.Entities;
using server_dotnet_dal.Repositories.Interfaces;

namespace server_dotnet.tests;

public class UserServiceBusinessLogicTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IOrganizationRepository> _orgRepoMock = new();
    private readonly IMapper _mapper;
    private readonly UserService _service;

    public UserServiceBusinessLogicTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDto>();
            cfg.CreateMap<UserCreateUpdateDto, User>();
        });
        _mapper = config.CreateMapper();

        _service = new UserService(_userRepoMock.Object, _orgRepoMock.Object, _mapper);
    }

    [Fact]
    public async Task CreateAsync_Throws_When_FirstNameOrLastNameIsNullOrWhitespace()
    {
        var dto = new UserCreateUpdateDto { FirstName = "", LastName = "Last", OrganizationId = 1 };
        await _service.Invoking(s => s.CreateAsync(dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("First and last name are required.");

        dto = new UserCreateUpdateDto { FirstName = "First", LastName = " ", OrganizationId = 1 };
        await _service.Invoking(s => s.CreateAsync(dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("First and last name are required.");
    }

    [Fact]
    public async Task CreateAsync_Throws_When_OrganizationNotFound()
    {
        var dto = new UserCreateUpdateDto { FirstName = "First", LastName = "Last", OrganizationId = 1 };
        _orgRepoMock.Setup(r => r.GetByIdAsync(dto.OrganizationId)).ReturnsAsync((Organization?)null);

        await _service.Invoking(s => s.CreateAsync(dto))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Organization not found.");
    }

    [Fact]
    public async Task CreateAsync_Returns_UserDto_When_Successful()
    {
        var dto = new UserCreateUpdateDto { FirstName = "First", LastName = "Last", OrganizationId = 1 };
        _orgRepoMock.Setup(r => r.GetByIdAsync(dto.OrganizationId)).ReturnsAsync(new Organization());

        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.FirstName.Should().Be(dto.FirstName);
        result.LastName.Should().Be(dto.LastName);
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_FirstNameOrLastNameIsNullOrWhitespace()
    {
        var dto = new UserCreateUpdateDto { FirstName = "", LastName = "Last", OrganizationId = 1 };
        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("First and last name are required.");

        dto = new UserCreateUpdateDto { FirstName = "First", LastName = " ", OrganizationId = 1 };
        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("First and last name are required.");
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_UserNotFound()
    {
        var dto = new UserCreateUpdateDto { FirstName = "First", LastName = "Last", OrganizationId = 1 };
        _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);

        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User not found.");
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_OrganizationNotFound()
    {
        var dto = new UserCreateUpdateDto { FirstName = "First", LastName = "Last", OrganizationId = 1 };
        _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new User());
        _orgRepoMock.Setup(r => r.GetByIdAsync(dto.OrganizationId)).ReturnsAsync((Organization?)null);

        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Organization not found.");
    }

    [Fact]
    public async Task DeleteAsync_Throws_When_UserNotFound()
    {
        _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);

        await _service.Invoking(s => s.DeleteAsync(1))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User not found.");
    }
}
