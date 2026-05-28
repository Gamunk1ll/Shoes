using System;
using System.Collections.Generic;

namespace Shoes;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string? OrderStatus1 { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
