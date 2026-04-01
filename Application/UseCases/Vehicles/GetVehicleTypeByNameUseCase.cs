using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Vehicles
{
    public class GetVehicleTypeByNameUseCase
    {
        private readonly IVehicleTypeRepository _repository;
        public GetVehicleTypeByNameUseCase(IVehicleTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<VehicleTypeEntity?> ExecuteAsync(string name)
        {
            var vehicleType = await _repository.GetByNameAsync(name);
            if (vehicleType == null)
            {
                throw new InvalidOperationException($"No se encontro el tipo de vehiculo con el nombre: {name} ");
            }
            return vehicleType;
        }
    }
}
