using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public record TokenPayload(Guid Sub, string Email, UserRole Role);
    public record TokenPair(string AccessToken, string RefreshToken);
    public interface ITokenService
    {
        TokenPair Generate(TokenPayload payload);
        TokenPayload Verify(string token);
        TokenPair Refresh(string refreshToken);

    }
}
