using System;
using System.Collections.Generic;

namespace Shoes;

public partial class PickPoint
{
    public int Id { get; set; }

    public int? Index { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    public int? Building { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
