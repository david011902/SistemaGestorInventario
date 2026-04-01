using Data.Persistence;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class VehicleTypeRepository : Repository<VehicleTypeEntity, Guid>, IVehicleTypeRepository
    {
        public VehicleTypeRepository(ApplicationDbContext context) : base(context)
        {

        }
        public override async Task<IEnumerable<VehicleTypeEntity>> GetAllAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive)
                .AsNoTracking()//al no usar para modificar, mejora el rendimiento
                .OrderBy(p => p.NameVehicle)
                .ToListAsync();
        }
        //ISocketRepository implementacion
        public async Task<VehicleTypeEntity?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("El nombre no puede ser vacío", nameof(name));
            }
            var normalizedSku = name.Trim().ToUpperInvariant();
            return await _context.VehicleTypes
                .FirstOrDefaultAsync(p => p.NameVehicle.ToUpper() == normalizedSku);
        }


    }
}
