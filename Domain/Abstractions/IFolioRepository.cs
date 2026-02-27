using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface IFolioRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByFolioAsync(string folio);

        Task<bool> ExistsWithFolioAsync(string folio);
    }
}
