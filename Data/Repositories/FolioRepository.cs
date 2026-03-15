using Data.Persistence;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories
{
    //Implementacion de la interfaz FolioRepository
    public class FolioRepository : IFolioRepository<ProductEntity>
    {
        private readonly ApplicationDbContext _context;
        public FolioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsWithFolioAsync(string folio)
        {
            if(string.IsNullOrWhiteSpace(folio))
            {
                throw new ArgumentException("El folio no puede ser vacío", nameof(folio));
            }
            var normalizedFolio = folio.Trim().ToUpperInvariant();  
            return await _context.Sales.AnyAsync(s => s.Folio.ToUpper() == normalizedFolio);
        }

        public Task<ProductEntity?> GetByFolioAsync(string folio)
        {
            if(string.IsNullOrWhiteSpace(folio))
            {
                throw new ArgumentException("El folio no puede ser vacío", nameof(folio));
            }
            var normalizedFolio = folio.Trim().ToUpperInvariant();  
            return _context.Products
                .Include(p => p.SaleDetails)
                .ThenInclude(sd => sd.Sale)
                .FirstOrDefaultAsync(p => p.SaleDetails.Any(sd => sd.Sale.Folio.ToUpper() == normalizedFolio));
        }
    }
}
