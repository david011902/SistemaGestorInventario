using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Products
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Guid VehicleTypeId { get; set; }
        public Guid? SocketTypeId { get; set; }
        //public int CategoryId { get; set; }
      
    }
}
