using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface IAuthService
    {
        Task<TokenPair?> LoginAsync(string email, string password);
        Task<UserEntity> RegisterAsync(string name, string email, string password);
        TokenPair RefreshAsync(string refreshToken);

    }
}
