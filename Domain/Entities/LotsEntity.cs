using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class LotsEntity
    {
        public Guid Id {  get; private set; }
        public int ProductId { get; private set; }  
        public int InitialAmount { get; private set; }  
        public int CurrentAmount { get; private set; }
        public decimal PurchaseCost { get; private set; }
        public DateTime ArrivateDate { get; private set; }
        public string ? Supplier { get; private set; }  

       public LotsEntity(int productId, int initialAmount, decimal purchaseCost, DateTime arrivateDate, string? supplier)
        {
            ValidateInitialAmount(initialAmount);
            ValidatePurchaseCost(purchaseCost);
            ValidateArrivalDate(arrivateDate);
            Id = Guid.NewGuid();
            this.ProductId = productId;
            this.InitialAmount = initialAmount;
            this.CurrentAmount = initialAmount; // Al crear un lote, la cantidad actual es igual a la cantidad inicial
            this.PurchaseCost = purchaseCost;
            this.ArrivateDate = arrivateDate;
            this.Supplier = supplier?.Trim();
        }

        public void UpdateLot(int initialAmount, decimal purchaseCost, DateTime arrivateDate, string? supplier)
        {
            ValidateInitialAmount(initialAmount);
            ValidatePurchaseCost(purchaseCost);
            ValidateArrivalDate(arrivateDate);
            this.PurchaseCost = purchaseCost;
            this.ArrivateDate = arrivateDate;
            this.Supplier = supplier?.Trim();
        }

        // Reglas de negocio
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
        private void ValidateArrivalDate(DateTime date)
        {
            if (date > DateTime.Now.AddDays(1))
                throw new ArgumentException("La fecha de llegada no puede ser futura.");
        }
    }
}
