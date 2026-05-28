using System;
using System.Collections.Generic;

namespace Shoes;

public partial class OrderProduct
{
    public int? NumberOrder { get; set; }

    public int? Product { get; set; }

    public int? Amount { get; set; }

    public int Id { get; set; }

    public virtual Order? NumberOrderNavigation { get; set; }

    public virtual Product? ProductNavigation { get; set; }
}
