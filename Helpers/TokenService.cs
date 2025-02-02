﻿using CarMarketplaceWebApi.Models.Identity;
using CarMarketplaceWebApi.Models.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarMarketplaceWebApi.Helpers
{
    public class TokenService : ITokenSevice
    {
        private readonly TokenSettings _settings;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IOptions<TokenSettings> options, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _settings = options.Value;
        }

        public async Task<JwtSecurityToken> CreateToken(AppUser appUser,IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,appUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,appUser.Email),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));

            var token = new JwtSecurityToken
                (
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                expires: DateTime.Now.AddMinutes(_settings.TokenValidityInMinutes),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            await _userManager.AddClaimsAsync(appUser, claims);

            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            TokenValidationParameters tokenValidation = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key ?? string.Empty))
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, tokenValidation, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token not found");
            }
            return principal;
        }
    }
}



//    private readonly JWTToken jwtToken;

//    public JWTGenerator(IOptions<JWTToken> jwtToken)
//    {
//        this.jwtToken = jwtToken.Value;
//    }

//    public string CreateJwtToken(AppUser user)
//    {
//        if (jwtToken == null) throw new ArgumentNullException("Key cannot be null.");

//        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtToken.Key));
//        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//        var claims = new[]
//        {
//            new Claim(ClaimTypes.Email,user.Email),

//        };

//        var token = new JwtSecurityToken(jwtToken.Issuer,
//            jwtToken.Audience,
//            claims,
//            expires: DateTime.Now.AddHours(1),
//            signingCredentials: credentials);

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }
