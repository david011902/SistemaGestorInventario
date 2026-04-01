using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Sales
{
    public class ResponseDetailSaleDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtSale { get; set; }
        public decimal Subtotal { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSku { get; set; } = string.Empty;
        public Guid LotId { get; set; }
    }
}
