using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Vehicles
{
    public class GetAllVehicleTypeUseCase
    {
        private readonly IVehicleTypeRepository _repository;
            public GetAllVehicleTypeUseCase(IVehicleTypeRepository repository)
            {
                _repository = repository;
            }
    
            public async Task<IEnumerable<VehicleTypeEntity>> ExecuteAsync()
            {
                return await _repository.GetAllAsync();
            }
    }
}
