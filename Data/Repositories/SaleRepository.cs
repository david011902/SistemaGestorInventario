using Data.Persistence;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Data.Repositories
{
    public class SaleRepository : Repository<SaleEntity, Guid>, ISaleRepository
    {
        public SaleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task DeleteAsync(SaleEntity sale)
        {
            throw new InvalidOperationException("Las ventas no se pueden eliminar físicamente del sistema.");
        }

        public async Task<bool> ExistsWithFolioAsync(string folio)
        {
            if (string.IsNullOrWhiteSpace(folio)) return false;

            var normalizedFolio = folio.Trim().ToUpperInvariant();
            return await _dbSet.AnyAsync(s => s.Folio.ToUpper() == normalizedFolio);
        }

        public async Task<SaleEntity?> GetByFolioAsync(string folio)
        {
            if (string.IsNullOrWhiteSpace(folio)) return null;

            var normalizedFolio = folio.Trim().ToUpperInvariant();
            return await _dbSet
                .Include(s => s.Details) 
                    .ThenInclude(sd => sd.Product) 
                .FirstOrDefaultAsync(s => s.Folio.ToUpper() == normalizedFolio);
        }
    }
}

