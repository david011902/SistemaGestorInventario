using Domain.Abstractions;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace Data.Services
{
    public class JwtTokenService(IConfiguration config) : ITokenService
    {
        private readonly string _accessSecret = config["Jwt:AccessSecret"]!;
        private readonly string _refreshSecret = config["Jwt:RefreshSecret"]!;
        public TokenPair Generate(TokenPayload payload)
        {
            var accessToken = BuildToken(payload, _accessSecret, minutes: 15);
            var refreshToken = BuildToken(payload, _refreshSecret, minutes: 60 * 24 * 7);
            return new TokenPair(accessToken, refreshToken);
        }

        public TokenPair Refresh(string refreshToken)
        {
            var principal = ValidateToken(refreshToken, _refreshSecret);
            var payload = ExtractPayload(principal);
            return Generate(payload);
        }

        public TokenPayload Verify(string token)
        {
            var principal = ValidateToken(token, _accessSecret);
            return ExtractPayload(principal);
        }

        //Helpers 
        private string BuildToken(TokenPayload payload, string secret, int minutes)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub,   payload.Sub.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, payload.Email),
            new Claim(ClaimTypes.Role,               payload.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
        };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ClaimsPrincipal ValidateToken(string token, string secret)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            return new JwtSecurityTokenHandler().ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                },
                out _);
        }


        private static TokenPayload ExtractPayload(ClaimsPrincipal principal)
        {
            var subClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var emailClaim = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(subClaim) || string.IsNullOrEmpty(emailClaim) || string.IsNullOrEmpty(roleClaim))
            {
                throw new SecurityTokenException("Token invalido: faltan claims");
            }

            var sub = Guid.Parse(subClaim);
            var email = emailClaim;
            var role = Enum.Parse<UserRole>(roleClaim);

            return new TokenPayload(sub, email, role);
        }
    }
}
