using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SocketTypeEntity
    {
        public Guid Id { get; private set; }
        public string NameSocket { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public SocketTypeEntity(string nameSocket)
        {

            ValidateNameSocket(nameSocket);
            Id = Guid.NewGuid();
            NameSocket = nameSocket.Trim();
            IsActive = true;
        }

        public void UpdateSocket(string nameSocket, bool isActive = true)
        {
            ValidateNameSocket(nameSocket);
            NameSocket = nameSocket.Trim();
            IsActive = isActive;
        }
        //Regla de negocio
        public void Desactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("El tipo de vehiculo ya se encuentra desactivado");
            IsActive = false;
            DeletedAt = DateTime.UtcNow;
        }
        private void ValidateNameSocket(string nameSocket)
        {
            if (string.IsNullOrEmpty(nameSocket))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (nameSocket.Trim().Length < 2)
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres.", nameof(nameSocket));
            if (nameSocket.Trim().Length > 100)
                throw new ArgumentException("El nombre no debe ser mayor a 100 caracteres", nameof(nameSocket));
        }
    }
}
