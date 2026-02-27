using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface ISaleRepository : IFolioRepository<SaleEntity>
    {
        Task<SaleEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<SaleEntity>> GetAllAsync();
        Task AddAsync(SaleEntity sale);
        Task UpdateAsync(SaleEntity sale);
        Task DeleteAsync(SaleEntity sale);
        Task<int> SaveChangesAsync();
    }
}
