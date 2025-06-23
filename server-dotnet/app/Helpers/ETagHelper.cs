using System.Security.Cryptography;
using System.Text;

namespace server_dotnet.Helpers;

public static class ETagHelper
{
    public static string GenerateETag(string json)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        var eTag = Convert.ToBase64String(hashBytes);

        return $"\"{eTag}\"";
    }
}
