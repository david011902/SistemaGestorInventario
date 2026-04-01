using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface ISkuRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetBySkuAsync(string sku);

        Task<bool> ExistsWithSkuAsync(string sku);
    }
}
