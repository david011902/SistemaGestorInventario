using Data.Persistence;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class SkuRepository : ISkuRepository<ProductEntity>
    {
        private readonly ApplicationDbContext _context;
        public SkuRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ExistsWithSkuAsync(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku)) return false;
            var normalizedSku = sku.Trim().ToUpperInvariant();
            return await _context.Products
                .AnyAsync(p => p.Sku.ToUpper() == normalizedSku);
        }

        public async Task<ProductEntity?> GetBySkuAsync(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku)) return null;
            var normalizedSku = sku.Trim().ToUpperInvariant();

            return await _context.Products.FirstOrDefaultAsync(p => p.Sku.ToUpper() == normalizedSku);
        }
    }
}
