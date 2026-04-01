using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Vehicles
{
    public class GetVehicleTypeByIdUseCase
    {
        private readonly IVehicleTypeRepository _repository;
        public GetVehicleTypeByIdUseCase(IVehicleTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<VehicleTypeEntity?> ExecuteAsync(Guid id)
        {
            var vehicleType = await _repository.GetByIdAsync(id);
            if (vehicleType == null)
            {
                throw new InvalidOperationException($"No se encontro el tipo de vehiculo con el id: {id} ");
            }
            return vehicleType;
        }
    }
}
