using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface IProductRepository : IRepository<ProductEntity, Guid>, ISkuRepository<ProductEntity>
    {
        Task<IEnumerable<ProductEntity>> GetByNameAsync(string name);
    }
}
