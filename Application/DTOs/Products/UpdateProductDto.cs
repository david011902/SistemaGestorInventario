using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Products
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
      
    }
}
