using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface IRefreshTokenRepository
    {
        Task SaveAsync(RefreshToken token);
        Task<RefreshToken?> FindAsync(string token);
        Task RevokeAsync(string tokenS);
    }
}
