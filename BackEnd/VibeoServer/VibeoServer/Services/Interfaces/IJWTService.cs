using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VibeoServer.Models;

namespace VibeoServer.Services.Interfaces
{
    public interface IJWTService
    {
        public Task<string> GenerateAccessToken(User user);
        public (string, int) GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
