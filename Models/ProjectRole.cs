using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectRole
{
    public decimal Roleid { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ProjectUserLogin> ProjectUserLogins { get; set; } = new List<ProjectUserLogin>();
}
