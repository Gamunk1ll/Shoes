using System;
using System.Collections.Generic;

namespace Shoes;

public partial class Order
{
    public int Id { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int? PickPointAddress { get; set; }

    public int? UserFullname { get; set; }

    public int? Code { get; set; }

    public int? OrderStatus { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual OrderStatus? OrderStatusNavigation { get; set; }

    public virtual PickPoint? PickPointAddressNavigation { get; set; }

    public virtual User? UserFullnameNavigation { get; set; }
}
