using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Lots
{
    public class UpdateLotDto
    {
        public Guid Id { set; get; }
        public int NewActualQuantity { set; get; }
    }
}
