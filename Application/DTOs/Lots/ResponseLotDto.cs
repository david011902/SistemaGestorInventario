using System;
using System.Collections.Generic;
using System.Text;
using Application.DTOs.Products;
namespace Application.DTOs.Lots
{
    public class ResponseLotDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int InitialAmount { get; set; }
        public int CurrentAmount { get; set; }
        public decimal PurchaseCost { get; set; }
        public DateTime ArrivateDate { get; set; }
        public string? Supplier { get; set; }
        public bool IsActive { get; set; }
        public ResponseProductDto Product { get; set; } = null!;

    }
}
