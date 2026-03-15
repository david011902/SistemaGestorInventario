using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface IRepository <TEntity, TId> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(TId id);
        //Para listas y colecciones se implementa IEnumerable, para objetos individuales se implementa Task<TEntity>
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<int> SaveChangesAsync();
    }
}
