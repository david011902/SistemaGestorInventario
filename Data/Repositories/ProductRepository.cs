using Data.Persistence;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories
{
    //Implementacion de la interfaz IRepository
    public class ProductRepository : Repository<ProductEntity, Guid>, ISkuRepository<ProductEntity>
    {
        public ProductRepository(ApplicationDbContext context) : base(context) 
        {
        }

        public override async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive)
                .AsNoTracking()//al no usar para modificar, mejora el rendimiento
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Stock)
                .ToListAsync();
        }


        //ISkuRepository implementacion
        public async Task<ProductEntity?> GetBySkuAsync(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("El sku no puede ser vacío", nameof(sku));
            }

            var normalizedSku = sku.Trim().ToUpperInvariant();
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Sku.ToUpper() == normalizedSku);

        }
        public async Task<bool> ExistsWithSkuAsync(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                throw new ArgumentException("El sku no puede ser vacío", nameof(sku));
            }
            var normalizedSku = sku.Trim().ToUpperInvariant();
            return await _context.Products
                .AnyAsync(p => p.Sku.ToUpper() == normalizedSku);
        }
    }
}
