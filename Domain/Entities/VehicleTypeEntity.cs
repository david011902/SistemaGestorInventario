using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class VehicleTypeEntity
    {
        public Guid Id { get; private set; }
        public string NameVehicle { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public VehicleTypeEntity(string nameVehicle)
        {

            ValidateNameVehicle(nameVehicle);
            Id = Guid.NewGuid();
            NameVehicle = nameVehicle.Trim();
            IsActive = true;
        }

        public void UpdateVehicle(string nameVehicle, bool isActive=true)
        {
            ValidateNameVehicle(nameVehicle);
            NameVehicle = nameVehicle.Trim();
            IsActive = isActive;
        }
        //Regla de negocio
        public void Activate()
        {
            IsActive = true;
            DeletedAt = null;
        }
        public void Desactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("El tipo de vehiculo ya se encuentra desactivado");
            IsActive = false;
            DeletedAt = DateTime.UtcNow;
        }
        private void ValidateNameVehicle(string nameVehicle)
        {
            if (string.IsNullOrEmpty(nameVehicle))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (nameVehicle.Trim().Length < 3)
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres.", nameof(nameVehicle));
            if (nameVehicle.Trim().Length > 100)
                throw new ArgumentException("El nombre no debe ser mayor a 100 caracteres", nameof(nameVehicle));
        }

    }   
}
