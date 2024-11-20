using CarMarketplaceWebApi.Models.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarMarketplaceWebApi.Helpers
{
    public interface ITokenSevice
    {
        Task<JwtSecurityToken> CreateToken(AppUser appUser,IList<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token); 
    }
}
