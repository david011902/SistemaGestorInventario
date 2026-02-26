using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SaleEntity
    {
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public string Folio { get; private set; } = string.Empty;
        public decimal Total { get; private set; }
        public SaleStatus Status { get; private set; }

        //Relacion de una venta, ya que se puede comprar mas de un articulo
        private readonly List<SaleDetailEntity> _details = new List<SaleDetailEntity>();
        public IReadOnlyCollection<SaleDetailEntity> Details => _details;
        
        public SaleEntity(DateTime date, string folio, decimal total, SaleStatus status)
        {
            ValidateFolio(folio);
            Id = Guid.NewGuid();
            Date = date;
            Folio = folio.Trim().ToUpper();
            Total = total;
            Status = status;
        }

        //Reglas de negocio
        private void ValidateFolio(string folio)
        {
            if (string.IsNullOrWhiteSpace(folio))
                throw new ArgumentException("El Folio es obligatorio.");
        }
       
    }
}
