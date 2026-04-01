using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SaleDetailEntity
    {
        public Guid Id { get; private set; }
        public Guid SaleId { get; private set; }
        public Guid LotId { get; private set; }
        public Guid ProductId { get; private set; } 
        public int Quantity { get; private set; }
        public int ReturnedQuantity { get; set; } 
        public decimal PriceAtSale { get; private set; }
        public decimal Subtotal => (Quantity - ReturnedQuantity) * PriceAtSale;
        public int EffectiveQuantity => Quantity - ReturnedQuantity;
        public virtual LotsEntity Lot { get; private set; } = null!;
        public virtual ProductEntity Product { get; private set; } = null!; 
        public virtual SaleEntity Sale { get; private set; } = null!;

        public SaleDetailEntity(Guid saleId,Guid productId ,Guid lotId, int quantity, decimal priceAtSale)
        {
            ValidatePriceAtSale(priceAtSale);
            ValidateQuantity(quantity);
            Id = Guid.NewGuid();
            SaleId = saleId;
            ProductId = productId;
            LotId = lotId;
            Quantity = quantity;
            PriceAtSale = priceAtSale;
        }

        //Reglas de negocio
        private void ValidateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.");
        }
        public void ApplyReturn(int quantityToReturn)
        {
            if (quantityToReturn <= 0) return;

            if (quantityToReturn > EffectiveQuantity)
                throw new InvalidOperationException($"No puedes devolver {quantityToReturn} unidades porque solo quedan {EffectiveQuantity} en esta venta.");

            ReturnedQuantity += quantityToReturn;
        }
        private void ValidatePriceAtSale(decimal priceAtSale) 
        {
            if (priceAtSale < 0)
                throw new ArgumentException("El precio no puede ser negativo");
        }
    }
}
