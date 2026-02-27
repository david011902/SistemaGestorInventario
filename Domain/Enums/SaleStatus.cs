using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum SaleStatus
    {
        Pending = 0,
        Completed = 1,
        PartialReturn = 2,
        TotalReturn = 3,
        Cancelled = 4,
    }
}
