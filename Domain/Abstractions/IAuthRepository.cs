using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface IAuthRepository
    {
        Task<UserEntity?> FindByEmailAsync(string email);
        Task<UserEntity?> FindByIdAsync(Guid id);
        Task<UserEntity> SaveAsync(UserEntity user);
    }
}
