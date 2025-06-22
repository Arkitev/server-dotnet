namespace server_dotnet.Dtos;

public record OrganizationDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Industry { get; init; }
    public DateTime DateFounded { get; init; }
}

