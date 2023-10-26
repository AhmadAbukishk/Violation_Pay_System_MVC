using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectUserLogin
{
    public decimal Loginid { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal Userid { get; set; }

    public decimal Roleid { get; set; }

    public virtual ProjectRole? Role { get; set; }

    public virtual ProjectUser? User { get; set; }
}
