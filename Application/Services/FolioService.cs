using Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
    public class FolioService : IFolioService
    {
        public string GenerateFolio()
        {
            return $"Venta-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}";
        }
    }
}
