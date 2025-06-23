using Microsoft.AspNetCore.Mvc;
using server_dotnet.Authorization;

namespace server_dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwt;

    public AuthController(IJwtService jwt)
    {
        _jwt = jwt;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        if (dto.Email == "test@test.com" && dto.Password == "password")
        {
            var token = _jwt.GenerateToken(1, dto.Email);
            return Ok(new { token });
        }

        return Unauthorized("Invalid credentials");
    }
}

public record LoginDto(string Email, string Password);
