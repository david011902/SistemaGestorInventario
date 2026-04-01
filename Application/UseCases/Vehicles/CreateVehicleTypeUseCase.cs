using Application.DTOs.VehiclesType;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Vehicles
{
    public class CreateVehicleTypeUseCase
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public CreateVehicleTypeUseCase(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<VehicleTypeEntity> ExecuteAsync(CreateVehicleTypeDto dto)
        {
            var exists = await _vehicleTypeRepository.GetByNameAsync(dto.Name);
            if (exists != null)
            {
                throw new InvalidOperationException($"El tipo de vehiculo '{dto.Name}' ya existe.");
            }
            var vehicleType = new VehicleTypeEntity(dto.Name);
            await _vehicleTypeRepository.AddAsync(vehicleType);
            await _vehicleTypeRepository.SaveChangesAsync();
            return vehicleType;
        }

    }
}
