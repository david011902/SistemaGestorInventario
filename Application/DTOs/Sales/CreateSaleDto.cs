using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Sales
{
    public class CreateSaleDto
    {
        public List<SaleItemDto> Items { get; set; } = new List<SaleItemDto>();
    }
}
