using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface IVehicleTypeRepository : IRepository<VehicleTypeEntity, Guid>
    {
        Task<VehicleTypeEntity?> GetByNameAsync(string name);
    }
}
