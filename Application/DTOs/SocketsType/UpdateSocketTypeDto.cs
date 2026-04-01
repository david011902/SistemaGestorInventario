using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.SocketsType
{
    public class UpdateSocketTypeDto
    {
        //public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
