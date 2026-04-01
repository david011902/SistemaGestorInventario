using Application.DTOs.VehiclesType;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Vehicles
{
    public class UpdateVehicleTypeUseCase
    {
        private readonly IVehicleTypeRepository _repository;

        public UpdateVehicleTypeUseCase(IVehicleTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<VehicleTypeEntity?> ExecuteAsync(UpdateVehicleTypeDto dto, Guid id)
        {
            var vehicleType = await _repository.GetByIdAsync(id);
            if (vehicleType == null)
            {
                throw new InvalidOperationException($"No se encontro el tipo de vehiculo con el id: {id}");
            }
            vehicleType.UpdateVehicle(dto.Name, dto.IsActive);
            await _repository.UpdateAsync(vehicleType);
            await _repository.SaveChangesAsync();
            return vehicleType;
        }
    }
}
