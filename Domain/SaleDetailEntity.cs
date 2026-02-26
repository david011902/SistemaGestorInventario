using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class SaleDetailEntity
    {
        public Guid Id { get; private set; }
        public Guid SaleId { get; private set; }
        public Guid LotId { get; private set; }
        public int Quantity { get; private set; }
        public decimal PriceAtSale { get; private set; }
        public decimal Subtotal => Quantity * PriceAtSale;

        public SaleDetailEntity(Guid saleId, Guid lotId, int quantity, decimal priceAtSale)
        {
            ValidatePriceAtSale(priceAtSale);
            ValidateQuantity(quantity);
            Id = Guid.NewGuid();
            SaleId = saleId;
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

        private void ValidatePriceAtSale(decimal priceAtSale) 
        {
            if (priceAtSale < 0)
                throw new ArgumentException("El precio no puede ser negativo");
        }
    }
}
