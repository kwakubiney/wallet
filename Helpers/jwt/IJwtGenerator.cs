using System.Security.Claims;

namespace Auth.Helpers.Interfaces
{
    public interface IJwtGenerator
    {
        JWTGenerator AddClaim(Claim claim);

        string GetToken();
    }
}