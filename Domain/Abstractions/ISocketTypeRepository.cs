using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface ISocketTypeRepository : IRepository<SocketTypeEntity, Guid>
    {
        Task<IEnumerable<SocketTypeEntity?>> GetByNameAsync(string name);
    }
}
