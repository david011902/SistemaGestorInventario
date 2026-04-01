using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Sales
{
    public class ReturnItemDto
    {
        public required string ProductSku { get; set; }
        public int QuantityToReturn { get; set; }
    }
}
