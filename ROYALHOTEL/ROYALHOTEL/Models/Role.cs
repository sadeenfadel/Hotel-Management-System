using System;
using System.Collections.Generic;

namespace ROYALHOTEL.Models;

public partial class Role
{
    public decimal Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();
}
