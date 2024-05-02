using System;
using System.Collections.Generic;

namespace CoreBus.Models;

public partial class BusType
{
    public int BusTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public decimal SeatFear { get; set; }

    public int PassangerId { get; set; }

    public virtual Passanger Passanger { get; set; } = null!;
}
