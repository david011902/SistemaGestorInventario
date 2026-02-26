using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class VehicleType
    {
        public Guid id { get; private set; }
        public string NameVehicle { get; private set; } = string.Empty;
        
        public VehicleType(string nameVehicle)
        {

            ValidateNameVehicle(nameVehicle);
            id = Guid.NewGuid();
            NameVehicle = nameVehicle.Trim(); 
        }

        public void UpdateVehicle(string nameVehicle)
        {
            ValidateNameVehicle(nameVehicle);
            NameVehicle = nameVehicle.Trim();
        }
        //Regla de negocio
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
