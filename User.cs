using System;
using System.Collections.Generic;

namespace Shoes;

public partial class User
{
    public int? UserRole { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int Id { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual UserRole? UserRoleNavigation { get; set; }
}
