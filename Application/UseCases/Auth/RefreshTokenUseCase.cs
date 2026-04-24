using Application.DTOs.Auth;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Auth
{
    public class RefreshTokenUseCase(ITokenService tokenService)
    {
        public LoginResponseDto ExecuteAsync(RefreshRequestDto dto)
        {
            var tokens = tokenService.Refresh(dto.RefreshToken);

            return new LoginResponseDto
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            };
        }
    }
}
