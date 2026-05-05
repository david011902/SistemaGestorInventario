using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace Data.Services
{
    public class JwtTokenService(IConfiguration config, IRefreshTokenRepository refreshRepo) : ITokenService
    {
        private readonly string _accessSecret = config["Jwt:AccessSecret"]!;

        public async Task<TokenPair>  GenerateAsync(TokenPayload payload)
        {
            var accessToken = BuildToken(payload, _accessSecret, minutes: 15);

            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            await refreshRepo.SaveAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = payload.Sub,
                Email = payload.Email,
                Role = payload.Role,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            return new TokenPair(accessToken, refreshToken);
        }
        public async Task<TokenPair> RefreshAsync(string refreshToken)
        {
            var stored = await refreshRepo.FindAsync(refreshToken);

            if (stored is null || stored.IsRevoked || stored.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token inválido o expirado.");

            // Rotación: invalida el token usado
            await refreshRepo.RevokeAsync(refreshToken);

            // Necesitamos los datos del usuario para generar el nuevo par
            var payload = new TokenPayload(stored.UserId, stored.Email, stored.Role);
            return await GenerateAsync(payload);
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
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
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
