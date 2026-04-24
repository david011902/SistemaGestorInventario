using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class LotsEntity
    {
        public Guid Id {  get; private set; }
        public Guid ProductId { get; private set; }  
        public int InitialAmount { get; private set; }  
        public int CurrentAmount { get; private set; }
        public decimal PurchaseCost { get; private set; }
        public DateTime ArrivateDate { get; private set; }
        public string ? Supplier { get; private set; }  
        public bool IsActive { get; private set; } // Indica si el lote está activo

        public virtual ProductEntity Product { get; private set; } = null!; // Relación con el producto

       public LotsEntity(Guid productId, int initialAmount, decimal purchaseCost, string? supplier)
        {
            ValidateInitialAmount(initialAmount);
            ValidatePurchaseCost(purchaseCost);
            Id = Guid.NewGuid();
            this.ProductId = productId;
            this.InitialAmount = initialAmount;
            this.CurrentAmount = initialAmount; // Al crear un lote, la cantidad actual es igual a la cantidad inicial
            this.PurchaseCost = purchaseCost;
            this.ArrivateDate = DateTime.UtcNow;
            this.Supplier = supplier?.Trim();
            this.IsActive = true; // Por defecto, un lote recién creado está activo
        }

        public void UpdateLot(int initialAmount, decimal purchaseCost, string? supplier)
        {
            ValidateInitialAmount(initialAmount);
            ValidatePurchaseCost(purchaseCost);
            this.PurchaseCost = purchaseCost;
            this.Supplier = supplier?.Trim();
        }

        public void AddStock(int quantity)
        {
            CurrentAmount += quantity;
        }
        // Reglas de negocio
        public void AdjustStock(int newCurrentAmount)
        {
            if (newCurrentAmount < 0)
                throw new ArgumentException("La cantidad actual no puede ser negativa.", nameof(newCurrentAmount));
            this.CurrentAmount = newCurrentAmount;
        }
        public void SubtractStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La cantidad a restar debe ser mayor a cero.", nameof(quantity));
            if (quantity > CurrentAmount)
                throw new InvalidOperationException("No hay suficiente stock para restar la cantidad solicitada.");
            CurrentAmount -= quantity;
        }
        private void ValidateInitialAmount(int initialAmount)
        {
            if (initialAmount <= 0)
                throw new ArgumentException("La cantidad inicial debe ser mayor a cero.", nameof(initialAmount));
        }
        private void ValidatePurchaseCost(decimal purchaseCost)
        {
            if (purchaseCost < 0)
                throw new ArgumentException("El costo de compra no puede ser negativo.", nameof(purchaseCost));
        }
    }
}
