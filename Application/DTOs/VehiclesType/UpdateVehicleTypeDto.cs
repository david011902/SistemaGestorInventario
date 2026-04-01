using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.VehiclesType
{
    public class UpdateVehicleTypeDto
    {
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
