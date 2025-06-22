using AutoMapper;
using FluentAssertions;
using Moq;
using server_dotnet.Dtos;
using server_dotnet.Services;
using server_dotnet_dal.Entities;
using server_dotnet_dal.Repositories.Interfaces;

namespace server_dotnet.tests;

public class OrganizationServiceBusinessLogicTests
{
    private readonly Mock<IOrganizationRepository> _orgRepoMock = new();
    private readonly IMapper _mapper;
    private readonly OrganizationService _service;

    public OrganizationServiceBusinessLogicTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Organization, OrganizationDto>();
            cfg.CreateMap<OrganizationCreateUpdateDto, Organization>();
        });
        _mapper = config.CreateMapper();

        _service = new OrganizationService(_orgRepoMock.Object, _mapper);
    }

    [Fact]
    public async Task CreateAsync_Throws_When_NameIsNullOrWhitespace()
    {
        var dto = new OrganizationCreateUpdateDto { Name = "" };
        await _service.Invoking(s => s.CreateAsync(dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Organization name is required.");

        dto = new OrganizationCreateUpdateDto { Name = "  " };
        await _service.Invoking(s => s.CreateAsync(dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Organization name is required.");
    }

    [Fact]
    public async Task CreateAsync_Returns_OrganizationDto_When_Successful()
    {
        var dto = new OrganizationCreateUpdateDto { Name = "Org Name" };
        _orgRepoMock.Setup(r => r.AddAsync(It.IsAny<Organization>()))
            .ReturnsAsync((Organization o) => o);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Name.Should().Be(dto.Name);
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_NameIsNullOrWhitespace()
    {
        var dto = new OrganizationCreateUpdateDto { Name = "" };
        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Organization name is required.");

        dto = new OrganizationCreateUpdateDto { Name = " " };
        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Organization name is required.");
    }

    [Fact]
    public async Task UpdateAsync_Throws_When_OrganizationNotFound()
    {
        var dto = new OrganizationCreateUpdateDto { Name = "Org Name" };
        _orgRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Organization?)null);

        await _service.Invoking(s => s.UpdateAsync(1, dto))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Organization not found.");
    }

    [Fact]
    public async Task DeleteAsync_Throws_When_OrganizationNotFound()
    {
        _orgRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Organization?)null);

        await _service.Invoking(s => s.DeleteAsync(1))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Organization not found.");
    }
}
