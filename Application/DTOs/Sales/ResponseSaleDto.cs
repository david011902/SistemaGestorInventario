using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Sales
{
    public class ResponseSaleDto
    {
        public Guid Id { get; set; }
        public string Folio { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<ResponseDetailSaleDto> Details { get; set; } = new();
    }
}
