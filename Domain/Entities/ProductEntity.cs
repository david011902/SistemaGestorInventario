using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Net.Sockets;

namespace Domain.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Sku { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        //public int CategoryId { get; private set; }

        public Guid VehicleTypeId { get; set; }
        public virtual VehicleTypeEntity VehicleType { get; set; } = null!;

        public Guid? SocketTypeId { get; set; }
        public virtual SocketTypeEntity? SocketType { get; set; }
        public bool IsActive { get; private set; } = true;
        public DateTime? DeletedAt { get; private set; }
        private readonly List<LotsEntity> _lots = new();
        public virtual IReadOnlyCollection<LotsEntity> Lots => _lots;
        public int Stock => _lots.Sum(l => l.CurrentAmount);
        private readonly List<SaleDetailEntity> _saleDetails = new();
        public virtual IReadOnlyCollection<SaleDetailEntity> SaleDetails => _saleDetails;
        //Constructor 
        protected ProductEntity() { }
        public ProductEntity(string name, string sku, decimal price, Guid vehicleTypeId, Guid? socketTypeId = null)
        {
            ValidateName(name);
            ValidateSku(sku);
            ValidatePrice(price);
            ValidateVehicleType(vehicleTypeId);
            ValidateSocketType(socketTypeId);
            Id = Guid.NewGuid();
            Name = name.Trim();
            Sku = sku.Trim().ToUpper();
            Price = price;
            VehicleTypeId = vehicleTypeId;
            SocketTypeId = socketTypeId;
            //CategoryId = categoryId;
            IsActive = true;
            
        }


        public void UpdateProduct(string name, decimal price, Guid vehicleTypeId, Guid? socketTypeId)
        {
            ValidateName(name);
            ValidatePrice(price);
            ValidateVehicleType(vehicleTypeId);
            ValidateSocketType(socketTypeId);
            Name = name.Trim();
            Price = price;
            VehicleTypeId = vehicleTypeId;
            SocketTypeId = socketTypeId;
        }



        //Reglas de negocio
        public void Desactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("El producto ya se encuentra desactivado");
            IsActive = false;
            DeletedAt = DateTime.UtcNow;
        }
        private void ValidateSku(string sku)
        {
            if (string.IsNullOrEmpty(sku))
                throw new ArgumentException("SKU no puede estar vacío.");
            if (sku.Trim().Length < 2)
                throw new ArgumentException("El SKU debe tener al menos 3 caracteres.", nameof(sku));

            if (sku.Trim().Length > 20)
                throw new ArgumentException("El SKU no debe ser mayor a 20 caracteres", nameof(sku));
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("El nombre no puede estar vacío.");
            if (name.Trim().Length < 2)
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres.", nameof(name));
            if (name.Trim().Length > 100)
                throw new ArgumentException("El nombre no debe ser mayor a 100 caracteres", nameof(name));
        }
        private void ValidatePrice(decimal price) 
        {
            if (price < 0)
                throw new ArgumentException("El precio no puede ser negativo.", nameof(price));
        }
        private void ValidateVehicleType(Guid vehicleTypeId)
        {
            if (vehicleTypeId == Guid.Empty)
                throw new ArgumentException("El tipo de vehículo es obligatorio y debe ser un ID válido.", nameof(vehicleTypeId));
        }
        private void ValidateSocketType(Guid? socketTypeId)
        {
            if (socketTypeId.HasValue && socketTypeId.Value == Guid.Empty)
                throw new ArgumentException("Si se proporciona un socket, el ID no puede estar vacío.", nameof(socketTypeId));
        }

    }
}
