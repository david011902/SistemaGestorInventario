using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Sales
{
    public class ReturnSaleDto
    {
        public string Folio { get; set; } = string.Empty;
        public List<ReturnItemDto> ItemsToReturn { get; set; } = null!;
    }
}
