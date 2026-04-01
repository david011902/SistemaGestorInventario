using Data.Persistence;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Data.Repositories
{
    public class LotRepository : Repository<LotsEntity, Guid>, ILotRepository
    {
        public LotRepository(ApplicationDbContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<LotsEntity>> GetAllAsync()
        {
            return await _context.Lots
                .Include(l => l.Product) 
                    .ThenInclude(p => p.VehicleType)
                .Include(l => l.Product)
                    .ThenInclude(p => p.SocketType)
                .Where(l => l.IsActive)
                .AsNoTracking()
                .OrderByDescending(l => l.ArrivateDate) // Ordenar por fecha de llegada
                .ToListAsync();
        }
        public async Task<IEnumerable<LotsEntity>> GetActiveLotsByProductIdAsync(Guid productId)
        {
            return await _context.Lots
                .Where(l => l.ProductId == productId && l.IsActive)
                .ToListAsync();
        }

        public async Task<List<LotsEntity>> GetActiveLotsBySkuAsync(string sku)
        {
            return await _context.Lots
                .Include(l=>l.Product)
                .Where(l => l.Product.Sku == sku && l.IsActive)
                .ToListAsync();
        }
    }
}
