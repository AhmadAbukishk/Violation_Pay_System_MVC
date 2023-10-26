using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectLicense
{
    public decimal Licenseid { get; set; }

    public DateTime? Issdate { get; set; }

    public DateTime? Expdate { get; set; }

    public decimal Userid { get; set; }

    public virtual ProjectUser? User { get; set; }
}
