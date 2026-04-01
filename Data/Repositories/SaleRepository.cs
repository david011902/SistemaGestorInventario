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
            return await _dbSet.Include(s=>s.Details).AnyAsync(s => s.Folio.ToUpper() == normalizedFolio);
        }

        public async Task<SaleEntity?> GetByFolioAsync(string folio)
        {
            var normalizedFolio = folio.Trim().ToUpperInvariant();

            var sales = await _context.Sales
                .Include(s => s.Details)
                    .ThenInclude(sd => sd.Product)
                .Where(s => s.Folio == normalizedFolio)
                .ToListAsync();

            return sales.FirstOrDefault();
        }
        public override async Task<SaleEntity?> GetByIdAsync(Guid id)
        {
                return await _context.Sales
                .Include(s => s.Details)
                    .ThenInclude(sd => sd.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public  override async Task<IEnumerable<SaleEntity>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.Details)            
                    .ThenInclude(d => d.Product)  
                .AsNoTracking()                     
                .OrderByDescending(s => s.Date)     
                .ToListAsync();
        }
    }
}

