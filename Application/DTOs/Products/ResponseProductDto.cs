using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Products
{
    public class ResponseProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string VehicleTypeName { get; set; } = string.Empty; // Solo el nombre
        public string SocketTypeName { get; set; } = string.Empty;  // Solo el nombre
        public int Stock { get; set; }
        public bool IsActive { get; set; }
    }
}
