using System;
using System.Collections.Generic;

namespace Shoes;

public partial class Product
{
    public string? Article { get; set; }

    public string? Name { get; set; }

    public string? UnitMetric { get; set; }

    public decimal? Price { get; set; }

    public int? Supplier { get; set; }

    public int? Manufacturer { get; set; }

    public int? Category { get; set; }

    public decimal? Discount { get; set; }

    public int? Amount { get; set; }

    public string? Description { get; set; }

    public string? Photo { get; set; }

    public int Id { get; set; }

    public virtual Category? CategoryNavigation { get; set; }

    public virtual Manufacturer? ManufacturerNavigation { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Supplier? SupplierNavigation { get; set; }
}
