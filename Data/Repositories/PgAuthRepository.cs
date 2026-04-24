using Data.Persistence;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories
{
    public class PgAuthRepository(ApplicationDbContext context) : IAuthRepository
    {
        public async Task<UserEntity?> FindByEmailAsync(string email)
        {
            var row = await context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            return row is null ? null : MapToEntity(row);
        }

        public async Task<UserEntity?> FindByIdAsync(Guid id)
        {
            var row = await context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            return row is null ? null : MapToEntity(row);
        }

        public async Task<UserEntity> SaveAsync(UserEntity user)
        {
            var existing = await context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existing is null)
            {
                context.Users.Add(user);
            }
            else
            {
                context.Entry(existing).CurrentValues.SetValues(user);
                context.Users.Update(existing);
            }

            await context.SaveChangesAsync();
            return user;
        }

        //Mapeo entre BD y dominio 

        private static UserEntity MapToEntity(UserEntity row) =>
            UserEntity.Reconstitute(
                id: row.Id,
                name: row.Name,
                email: row.Email,
                passwordHash: row.PasswordHash,
                role: row.Role
                );
    }
}
