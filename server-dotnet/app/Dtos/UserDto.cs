namespace server_dotnet.Dtos;

public record UserDto
{
    public int Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public int OrganizationId { get; init; }
    public DateTime DateCreated { get; init; }
}
