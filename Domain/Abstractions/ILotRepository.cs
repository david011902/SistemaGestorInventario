using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface ILotRepository : IRepository<LotsEntity, Guid>
    {
        Task<IEnumerable<LotsEntity>> GetActiveLotsByProductIdAsync(Guid productId);
    }
}
