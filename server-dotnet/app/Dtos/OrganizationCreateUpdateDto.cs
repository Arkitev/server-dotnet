namespace server_dotnet.Dtos;

public record OrganizationCreateUpdateDto
{
    public string Name { get; init; } = null!;
    public string? Industry { get; init; }
}
