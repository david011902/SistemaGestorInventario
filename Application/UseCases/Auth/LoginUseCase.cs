using Application.DTOs.Auth;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Auth
{
    public class LoginUseCase(IAuthRepository authRepository, ITokenService tokenService)
    {
        public async Task<LoginResponseDto> ExecuteAsync(LoginRequestDto dto)
        {
            var user = await authRepository.FindByEmailAsync(dto.Email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Credenciales inválidas.");

            var payload = new TokenPayload(user.Id, user.Email, user.Role);
            var tokens = tokenService.Generate(payload);

            return new LoginResponseDto
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            };
        }
    }
}
