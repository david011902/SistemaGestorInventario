using Data.Persistence;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories
{
    public class SocketTypeRepository : Repository<SocketTypeEntity, Guid>, ISocketTypeRepository 
    {
        public SocketTypeRepository(ApplicationDbContext context) : base(context)
        {

        }
        public override async Task<IEnumerable<SocketTypeEntity>> GetAllAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive)
                .AsNoTracking()//al no usar para modificar, mejora el rendimiento
                .OrderBy(p => p.NameSocket)
                .ToListAsync();
        }

        public async Task<IEnumerable<SocketTypeEntity?>>GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("El nombre no puede ser vacío", nameof(name));
            }
            var normalizedSku = name.Trim().ToUpperInvariant();
            return await _context.SocketTypes
                .Where(p=>p.NameSocket.ToUpper().Contains(normalizedSku)).ToListAsync();
        }
    }
}
