namespace server_dotnet.Authorization;

public interface IJwtService
{
    string GenerateToken(int userId, string email);
}
