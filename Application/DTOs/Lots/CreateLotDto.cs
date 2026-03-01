using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Lots
{
    public class CreateLotDto
    {
        public Guid ProductId { get; set; }
        public int InitialAmount { get; set; }
        public decimal PurchaseCost { get; set; }
        public DateTime ArrivatelDate { get; set; }
        public string? Supplier { get; set; }
    }
}
