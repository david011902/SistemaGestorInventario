using Application.DTOs.Auth;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Auth
{
    public class RefreshTokenUseCase(ITokenService tokenService)
    {
        public async Task<TokenPair> ExecuteAsync(string refreshToken)
        {
            return await tokenService.RefreshAsync(refreshToken);
        }
    }
}
