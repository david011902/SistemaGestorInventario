using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public enum SaleStatus
    {
        Completed = 1,
        PartialReturn = 2,
        TotalReturn = 3,
        Cancelled = 4,
    }
}
