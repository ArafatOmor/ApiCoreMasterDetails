using System;
using System.Collections.Generic;

namespace CoreBus.Models;

public partial class Passanger
{
    public int PassangerId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsPaid { get; set; }

    public DateTime JournyDate { get; set; }

    public string? ImageName { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<BusType> BusTypes { get; set; } = new List<BusType>();
}
