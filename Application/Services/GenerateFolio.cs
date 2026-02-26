using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
    public static class GenerateFolio
    {
        public static string InventoryFolio()
        {
            return $"Venta-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}";
        }
    }
}
