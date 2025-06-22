namespace server_dotnet.Dtos;

public record OrderCreateUpdateDto
{
    public decimal TotalAmount { get; init; }
    public int UserId { get; init; }
    public int OrganizationId { get; init; }
}
