using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SaleEntity
    {
        public Guid Id { get; private set; }
        public Guid SaleId { get; private set; }
        public DateTime Date { get; private set; }
        public string Folio { get; private set; } = string.Empty;
        public decimal Total { get; private set; }
        public SaleStatus Status { get; private set; }
        public virtual SaleEntity Sale { get; private set; } = null!;
        //Relacion de una venta, ya que se puede comprar mas de un articulo
        private readonly List<SaleDetailEntity> _details = new List<SaleDetailEntity>();
        public IReadOnlyCollection<SaleDetailEntity> Details => _details;
        
        public SaleEntity(string folio)
        {
            ValidateFolio(folio);
            Id = Guid.NewGuid();
            Date = DateTime.UtcNow;
            Folio = folio.Trim().ToUpper();
            Total = 0;
            Status = SaleStatus.Pending;
        }

        public void Cancel()
        {
            Status = SaleStatus.Cancelled;
        }
        //Reglas de negocio
        public void AddDetail(Guid id, int quantity, decimal unitPrice)
        {
            var detail = new SaleDetailEntity(this.Id, id, quantity, unitPrice);
            _details.Add(detail);
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            Total = _details.Sum(d => d.Subtotal);
        }

        private void ValidateFolio(string folio)
        {
            if (string.IsNullOrWhiteSpace(folio))
                throw new ArgumentException("El Folio es obligatorio.");
        }
       
    }
}
