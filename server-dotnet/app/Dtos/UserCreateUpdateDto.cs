namespace server_dotnet.Dtos;

public record UserCreateUpdateDto
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public int OrganizationId { get; init; }
}
