using Data.Persistence;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Data.Repositories
{
    //Implementacion de la interfaz IRepository
    public class ProductRepository : Repository<ProductEntity, Guid>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) 
        {
        }
        public override async Task<ProductEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.VehicleType)
                .Include(p => p.SocketType)
                .Include(p => p.Lots)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public override async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            return await _context.Products
                .Include(p=>p.VehicleType)
                .Include(p=>p.SocketType)
                .Include(p => p.Lots)
                .Where(p => p.IsActive)
                .AsNoTracking()//al no usar para modificar, mejora el rendimiento
                .OrderBy(p => p.Name)
                //.ThenBy(p => p.Stock)
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
                .Include(p=>p.VehicleType)
                .Include(p=>p.SocketType)
                .Include(p=>p.Lots)
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
                .Include(p=>p.VehicleType)
                .Include(p=>p.SocketType)
                .Include(p => p.Lots)
                .AnyAsync(p => p.Sku.ToUpper() == normalizedSku);
        }

        public async Task<IEnumerable<ProductEntity>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<ProductEntity>();

            return await _context.Products
                .Include(p => p.VehicleType)
                .Include(p => p.SocketType)
                .Include(p => p.Lots)
                .Where(p => p.Name.ToLower().Contains(name.ToLower()) && p.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

